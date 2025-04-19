// Arquivo: Aplicacao/UseCase/Pedido/Criar/CriarPedidoUseCase.cs

using AutoMapper;
using Dominio.Entidades;
using Dominio.Enums;
using Dominio.Repositorios;
using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios.Endereco;
using Dominio.Repositorios.Pagamento; // Certifique-se que IPagamentoWriteOnly tem o método Adicionar
using Dominio.Repositorios.Pedido;
using Dominio.Repositorios.Usuario;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.Pedido.Criar;

public class CriarPedidoUseCase : ICriarPedidoUseCase
{
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly ICarrinhoWriteOnly _carrinhoWriteOnly;
	private readonly IUsuarioReadOnly _usuarioReadOnly;
	private readonly IEnderecoReadOnly _enderecoReadOnly;
	private readonly IPedidoWriteOnly _pedidoWriteOnly;
	private readonly IPagamentoWriteOnly _pagamentoWriteOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public CriarPedidoUseCase(
		ICarrinhoReadOnly carrinhoReadOnly,
		ICarrinhoWriteOnly carrinhoWriteOnly,
		IUsuarioReadOnly usuarioReadOnly,
		IEnderecoReadOnly enderecoReadOnly,
		IPedidoWriteOnly pedidoWriteOnly,
		IPagamentoWriteOnly pagamentoWriteOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper)
	{
		_carrinhoReadOnly = carrinhoReadOnly;
		_carrinhoWriteOnly = carrinhoWriteOnly;
		_usuarioReadOnly = usuarioReadOnly;
		_enderecoReadOnly = enderecoReadOnly;
		_pedidoWriteOnly = pedidoWriteOnly;
		_pagamentoWriteOnly = pagamentoWriteOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<ResponsePedidoCriadoJson> Execute(RequestCheckoutJson request)
	{
		// 1. Validações Iniciais (lançarão exceção se falharem)
		Usuario usuario = await _usuarioReadOnly.GetById(request.UsuarioId)
			?? throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado.");

		Endereco endereco = await _enderecoReadOnly.GetById(request.UsuarioId, request.EnderecoId)
			 ?? throw new NotFoundException($"Endereço com ID {request.EnderecoId} não encontrado para este usuário.");

		CarrinhoDeCompras? carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(request.UsuarioId);

		if (carrinho == null || !carrinho.ItensCarrinho.Any())
		{
			throw new ErrorOnValidationException(["O carrinho está vazio."]);
		}

		Dominio.Entidades.Pedido novoPedido = new()
		{
			UsuarioId = request.UsuarioId,
			EnderecoId = request.EnderecoId,
			DataPedido = DateTime.UtcNow,
			Status = StatusPedido.Pendente,
			ValorTotal = 0
		};

		foreach (var itemCarrinho in carrinho.ItensCarrinho)
		{
			if (itemCarrinho.Produto is null)
			{
				throw new InvalidOperationException($"Produto ID {itemCarrinho.ProdutoId} não carregado no carrinho.");
			}
			ItemPedido itemPedido = _mapper.Map<ItemPedido>(itemCarrinho);
			itemPedido.Produto = null;
			novoPedido.Itens.Add(itemPedido);
			novoPedido.ValorTotal += itemPedido.Quantidade * itemPedido.PrecoUnitario;
		}

		Pagamento novoPagamento = new()
		{
			Pedido = novoPedido,
			MetodoPagamento = request.MetodoPagamento,
			StatusPagamento = StatusPagamento.Pendente,
			Valor = novoPedido.ValorTotal,
			DataPagamento = DateTime.UtcNow
		};

		await _pedidoWriteOnly.Adicionar(novoPedido);
		await _pagamentoWriteOnly.Adicionar(novoPagamento);

		await _carrinhoWriteOnly.RemoverItemCarrinho(carrinho.Id);

		await _unitOfWork.Commit();

		long pedidoId = novoPedido.Id;
		long pagamentoId = novoPagamento.Id;

		if (pagamentoId == 0)
		{
			throw new InvalidOperationException($"Falha crítica na criação do pedido: Pagamento associado ao Pedido ID {pedidoId} não recebeu um ID válido do banco de dados após o commit.");
		}

		var statusPagamentoSimulado = StatusPagamento.Aprovado;
		var statusPedidoSimulado = StatusPedido.Processando;

		await _pagamentoWriteOnly.AtualizarStatus(pagamentoId, statusPagamentoSimulado);
		await _pedidoWriteOnly.AtualizarStatus(pedidoId, statusPedidoSimulado);

		await _unitOfWork.Commit();

		return new ResponsePedidoCriadoJson
		{
			PedidoId = pedidoId,
			StatusInicial = statusPedidoSimulado.ToString()
		};
	}
}