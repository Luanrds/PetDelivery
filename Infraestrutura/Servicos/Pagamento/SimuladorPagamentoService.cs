using Dominio.Entidades;
using Dominio.Enums;
using Dominio.Repositorios;
using Dominio.Repositorios.Pagamento;
using Dominio.Repositorios.Pedido;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Pagamento;
using Microsoft.Extensions.Logging;

namespace Infraestrutura.Servicos.Pagamento;
public class SimuladorPagamentoService : ISimuladorPagamentoService
{
	private readonly IPedidoReadOnly _pedidoReadOnly;
	private readonly IPedidoWriteOnly _pedidoWriteOnly;
	private readonly IPagamentoWriteOnly _pagamentoWriteOnly;
	private readonly IProdutoReadOnly _produtoReadOnly; // Para verificar estoque antes de atualizar
	private readonly IProdutoUpdateOnly _produtoUpdateOnly; // Para buscar com tracking e atualizar
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<SimuladorPagamentoService> _logger;

	public SimuladorPagamentoService(
		IPedidoReadOnly pedidoReadOnly,
		IPedidoWriteOnly pedidoWriteOnly,
		IPagamentoWriteOnly pagamentoWriteOnly,
		IProdutoReadOnly produtoReadOnly,
		IProdutoUpdateOnly produtoUpdateOnly,
		IUnitOfWork unitOfWork,
		ILogger<SimuladorPagamentoService> logger)
	{
		_pedidoReadOnly = pedidoReadOnly;
		_pedidoWriteOnly = pedidoWriteOnly;
		_pagamentoWriteOnly = pagamentoWriteOnly;
		_produtoReadOnly = produtoReadOnly;
		_produtoUpdateOnly = produtoUpdateOnly;
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public async Task SimularConfirmacaoPagamentoAsync(long pedidoId)
	{
		_logger.LogInformation("Processando simulação de pagamento para Pedido ID: {PedidoId}", pedidoId);

		// GetByIdAsync já deve incluir Pagamento e Itens com ThenInclude(i => i.Produto)
		Pedido? pedido = await _pedidoReadOnly.GetByIdAsync(pedidoId);

		if (pedido is null)
		{
			_logger.LogWarning("Pedido {PedidoId} não encontrado durante simulação de pagamento.", pedidoId);
			// Poderia lançar NotFoundException, mas no contexto de um background service,
			// talvez seja melhor apenas logar e seguir, pois o pedido pode ter sido excluído.
			return;
		}

		if (pedido.Pagamento is null)
		{
			_logger.LogError("Pedido {PedidoId} não possui registro de pagamento associado.", pedidoId);
			// Lançar ou logar e retornar? Depende da regra de negócio.
			return;
		}

		if (pedido.Pagamento.StatusPagamento != StatusPagamento.Pendente)
		{
			_logger.LogInformation("Pagamento para Pedido ID: {PedidoId} já está processado (Status: {Status}). Simulação ignorada.", pedidoId, pedido.Pagamento.StatusPagamento);
			return;
		}

		_logger.LogDebug("Simulando atraso no processamento do pagamento para Pedido ID: {PedidoId}...", pedidoId);
		await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(2, 6))); // Simula tempo de processamento

		bool aprovado = Random.Shared.Next(100) < 90; // 90% de chance de aprovar
		StatusPagamento novoStatusPagamento = aprovado ? StatusPagamento.Aprovado : StatusPagamento.Reprovado;
		StatusPedido novoStatusPedido = aprovado ? StatusPedido.Processando : StatusPedido.Cancelado;

		_logger.LogInformation("Resultado da simulação para Pedido ID: {PedidoId} - Pagamento: {StatusPagamento}, Status Pedido: {StatusPedido}", pedidoId, novoStatusPagamento, novoStatusPedido);

		try
		{
			// Atualiza os status primeiro
			pedido.Pagamento.StatusPagamento = novoStatusPagamento;
			pedido.Status = novoStatusPedido;

			// Delega a atualização para os repositórios WriteOnly
			// Nota: A implementação atual de AtualizarStatus pode buscar o pedido novamente.
			// Seria mais otimizado se AtualizarStatus recebesse a entidade já modificada.
			// Vamos assumir que AtualizarStatus funciona corretamente por enquanto.
			await _pagamentoWriteOnly.AtualizarStatus(pedido.Pagamento.Id, novoStatusPagamento);
			await _pedidoWriteOnly.AtualizarStatus(pedido.Id, novoStatusPedido);

			if (aprovado)
			{
				_logger.LogInformation("Pagamento Aprovado. Iniciando atualização de estoque para Pedido ID: {PedidoId}...", pedidoId);
				// Atualiza o estoque item por item
				foreach (var item in pedido.Itens)
				{
					// Busca o produto com tracking para atualização
					Produto produto = await _produtoUpdateOnly.GetById(item.ProdutoId)
						?? throw new InvalidOperationException($"Produto {item.ProdutoId} não encontrado ao tentar atualizar estoque para Pedido {pedidoId}.");

					if (item.Quantidade > produto.QuantidadeEstoque)
					{
						// ESTOQUE INSUFICIENTE! Isso indica um problema, pois a verificação inicial (se feita) deveria ter pego.
						// Ou pode ser um caso de concorrência se não estiver implementado.
						// DECISÃO: Cancelar o pedido OU logar erro crítico? Vamos cancelar.
						_logger.LogError("Estoque insuficiente detectado para Produto ID: {ProdutoId} ({ProdutoNome}) ao confirmar pagamento do Pedido ID: {PedidoId}. Estoque: {Estoque}, Qtd Pedido: {Qtd}. Cancelando o pedido.", produto.Id, produto.Nome, pedidoId, produto.QuantidadeEstoque, item.Quantidade);

						pedido.Status = StatusPedido.Cancelado;
						pedido.Pagamento.StatusPagamento = StatusPagamento.Reprovado; // Ou um status específico de erro de estoque
						await _pedidoWriteOnly.AtualizarStatus(pedido.Id, StatusPedido.Cancelado);
						await _pagamentoWriteOnly.AtualizarStatus(pedido.Pagamento.Id, StatusPagamento.Reprovado);
						await _unitOfWork.Commit(); // Salva o cancelamento
						return; // Sai do método após cancelar
					}

					produto.QuantidadeEstoque -= item.Quantidade;
					_produtoUpdateOnly.Atualize(produto); // Marca para salvar a alteração no estoque
					_logger.LogDebug("Estoque do Produto ID: {ProdutoId} ({ProdutoNome}) marcado para atualização para {NovoEstoque} (Pedido ID: {PedidoId})", produto.Id, produto.Nome, produto.QuantidadeEstoque, pedidoId);
				}
			}

			// Salva TODAS as alterações pendentes (status do pedido, status do pagamento E estoque dos produtos)
			await _unitOfWork.Commit();
			_logger.LogInformation("Alterações para Pedido ID: {PedidoId} salvas com sucesso (Status Pag: {StatusPagamento}, Status Ped: {StatusPedido}, Estoque atualizado: {EstoqueAtualizado})",
								  pedidoId, novoStatusPagamento, novoStatusPedido, aprovado);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Falha ao salvar atualizações para Pedido ID: {PedidoId} após simulação de pagamento.", pedidoId);
			// Considerar lógica de compensação/rollback se necessário (embora UnitOfWork já faça rollback da transação)
			throw; // Relança para o HostedService lidar (ou logar)
		}
	}
}
