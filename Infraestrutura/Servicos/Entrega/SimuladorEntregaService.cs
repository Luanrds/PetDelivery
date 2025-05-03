using Dominio.Entidades;
using Dominio.Enums;
using Dominio.Repositorios.Pedido;
using Dominio.Repositorios;
using Dominio.Servicos.Entrega;
using Microsoft.Extensions.Logging;

namespace Infraestrutura.Servicos.Entrega;
public class SimuladorEntregaService : ISimuladorEntregaService
{
	private readonly IPedidoReadOnly _pedidoReadOnly;
	private readonly IPedidoWriteOnly _pedidoWriteOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ILogger<SimuladorEntregaService> _logger;

	public SimuladorEntregaService(
		IPedidoReadOnly pedidoReadOnly,
		IPedidoWriteOnly pedidoWriteOnly,
		IUnitOfWork unitOfWork,
		ILogger<SimuladorEntregaService> logger)
	{
		_pedidoReadOnly = pedidoReadOnly;
		_pedidoWriteOnly = pedidoWriteOnly;
		_unitOfWork = unitOfWork;
		_logger = logger;
	}

	public async Task SimularEtapasEntregaAsync(long pedidoId)
	{
		_logger.LogInformation("Processando simulação de entrega para Pedido ID: {PedidoId}", pedidoId);

		// Busca o pedido. Assume-se que GetByIdAsync não precisa incluir relações aqui.
		Pedido? pedido = await _pedidoReadOnly.GetByIdAsync(pedidoId);

		if (pedido is null)
		{
			_logger.LogWarning("Pedido {PedidoId} não encontrado durante simulação de entrega.", pedidoId);
			return;
		}

		try
		{
			// Etapa 1: De Processando para Enviado
			if (pedido.Status == StatusPedido.Processando)
			{
				_logger.LogDebug("Simulando tempo de preparo para envio do Pedido ID: {PedidoId}...", pedidoId);
				await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(5, 11))); // Tempo de preparo

				_logger.LogInformation("Atualizando Pedido ID: {PedidoId} para Enviado.", pedidoId);
				await _pedidoWriteOnly.AtualizarStatus(pedido.Id, StatusPedido.Enviado);
				// Poderia setar uma DataEnvio aqui
				await _unitOfWork.Commit();
				pedido.Status = StatusPedido.Enviado; // Atualiza estado local para a próxima verificação
			}

			// Etapa 2: De Enviado para Concluido
			if (pedido.Status == StatusPedido.Enviado)
			{
				_logger.LogDebug("Simulando tempo de trânsito para entrega do Pedido ID: {PedidoId}...", pedidoId);
				await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(10, 21))); // Tempo de entrega

				_logger.LogInformation("Atualizando Pedido ID: {PedidoId} para Concluido.", pedidoId);
				await _pedidoWriteOnly.AtualizarStatus(pedido.Id, StatusPedido.Concluido);
				// Poderia setar uma DataConclusao aqui
				await _unitOfWork.Commit();
				pedido.Status = StatusPedido.Concluido; // Atualiza estado local

				// ---- SIMULAÇÃO DE NOTIFICAÇÃO ----
				_logger.LogInformation("[NOTIFICACAO SIMULADA] Pedido {PedidoId} foi concluído/entregue.", pedidoId);
				// Aqui entraria a lógica para enviar email, push, etc.
				// E talvez marcar um flag `NotificacaoEnviada = true` no pedido.
			}
			else if (pedido.Status != StatusPedido.Concluido && pedido.Status != StatusPedido.Cancelado) // Evita logar para pedidos já finalizados/cancelados
			{
				_logger.LogInformation("Pedido ID: {PedidoId} não está em Processando ou Enviado (Status: {Status}). Nenhuma ação de entrega realizada.", pedidoId, pedido.Status);
			}
			_logger.LogInformation("Simulação de entrega para Pedido ID: {PedidoId} finalizada.", pedidoId);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Falha ao salvar atualizações de entrega para Pedido ID: {PedidoId}.", pedidoId);
			throw; // Relança para o HostedService
		}
	}
}