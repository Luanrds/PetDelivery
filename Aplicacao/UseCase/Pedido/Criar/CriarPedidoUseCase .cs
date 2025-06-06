using AutoMapper;
using Dominio.Entidades;
using Dominio.Enums;
using Dominio.Repositorios;
using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios.Endereco;
using Dominio.Repositorios.Pagamento;
using Dominio.Repositorios.Pedido;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.Pedido.Criar;

public class CriarPedidoUseCase : ICriarPedidoUseCase
{
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly ICarrinhoWriteOnly _carrinhoWriteOnly;
	private readonly IEnderecoReadOnly _enderecoReadOnly;
	private readonly IPedidoWriteOnly _pedidoWriteOnly;
	private readonly IPagamentoWriteOnly _pagamentoWriteOnly;
	private readonly IProdutoReadOnly _produtoReadOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado;

	public CriarPedidoUseCase(
		ICarrinhoReadOnly carrinhoReadOnly,
		ICarrinhoWriteOnly carrinhoWriteOnly,
		IEnderecoReadOnly enderecoReadOnly,
		IPedidoWriteOnly pedidoWriteOnly,
		IPagamentoWriteOnly pagamentoWriteOnly,
		IProdutoReadOnly produtoReadOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper,
		IUsuarioLogado usuarioLogado)
	{
		_carrinhoReadOnly = carrinhoReadOnly;
		_carrinhoWriteOnly = carrinhoWriteOnly;
		_enderecoReadOnly = enderecoReadOnly;
		_pedidoWriteOnly = pedidoWriteOnly;
		_pagamentoWriteOnly = pagamentoWriteOnly;
		_produtoReadOnly = produtoReadOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
	}

	public async Task<ResponsePedidoCriadoJson> Execute(RequestCheckoutJson request)
	{
		Usuario usuario = await _usuarioLogado.Usuario();

		_ = await _enderecoReadOnly.GetById(usuario.Id, request.EnderecoId)
			?? throw new NotFoundException($"Endereço com ID {request.EnderecoId} não encontrado ou não pertence a este usuário.");

		CarrinhoDeCompras carrinho = await ObterCarrinhoValido(usuario.Id);

		Dominio.Entidades.Pedido pedido = await CriarPedido(carrinho, usuario.Id, request);

		await FinalizarPedido(pedido, carrinho.Id);

		return new ResponsePedidoCriadoJson
		{
			PedidoId = pedido.Id,
			StatusInicial = pedido.Status.ToString()
		};
	}

	private async Task<CarrinhoDeCompras> ObterCarrinhoValido(long usuarioId)
	{
		CarrinhoDeCompras? carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(usuarioId);

		if (carrinho == null || carrinho.ItensCarrinho == null || carrinho.ItensCarrinho.Count == 0)
			throw new ErrorOnValidationException(["O carrinho está vazio."]);

		return carrinho;
	}

	private async Task<Dominio.Entidades.Pedido> CriarPedido(CarrinhoDeCompras carrinho, long usuarioId, RequestCheckoutJson request)
	{
		Dominio.Entidades.Pedido pedido = new()
		{
			UsuarioId = usuarioId,
			EnderecoId = request.EnderecoId,
			DataPedido = DateTime.UtcNow,
			Status = StatusPedido.Pendente,
			Itens = [],
			ValorTotal = 0
		};

		foreach (ItemCarrinhoDeCompra itemCarrinho in carrinho.ItensCarrinho)
		{
			Produto produto = await VerificarProdutoNoEstoque(itemCarrinho);
			ItemPedido itemPedido = CriarItemPedido(itemCarrinho, produto);

			pedido.Itens.Add(itemPedido);
			pedido.ValorTotal += itemPedido.SubTotal;
		}

		pedido.Pagamento = new Pagamento
		{
			MetodoPagamento = request.MetodoPagamento,
			StatusPagamento = StatusPagamento.Pendente,
			Valor = pedido.ValorTotal,
			DataPagamento = DateTime.UtcNow
		};

		await _pedidoWriteOnly.Adicionar(pedido);
		await _unitOfWork.Commit();

		return pedido;
	}

	private async Task<Produto> VerificarProdutoNoEstoque(ItemCarrinhoDeCompra item)
	{
		Produto produto = item.Produto ?? await _produtoReadOnly.GetById(item.ProdutoId)
			?? throw new InvalidOperationException($"Produto com ID {item.ProdutoId} não encontrado.");

		if (item.Quantidade > produto.QuantidadeEstoque)
			throw new ErrorOnValidationException([$"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.QuantidadeEstoque}."]);

		return produto;
	}

	private static ItemPedido CriarItemPedido(ItemCarrinhoDeCompra itemCarrinho, Produto produto) => new()
	{
		ProdutoId = produto.Id,
		Produto = produto,
		Quantidade = itemCarrinho.Quantidade,
		PrecoUnitario = produto.Valor
	};

	private async Task FinalizarPedido(Dominio.Entidades.Pedido pedido, long carrinhoId)
	{
		await _carrinhoWriteOnly.LimparItensAsync(carrinhoId);

		if (pedido.Pagamento?.Id == 0)
			throw new InvalidOperationException($"Falha crítica: pagamento do pedido {pedido.Id} não foi salvo corretamente.");

		StatusPagamento statusPagamento = StatusPagamento.Aprovado;
		await _pagamentoWriteOnly.AtualizarStatus(pedido.Pagamento!.Id, statusPagamento);

		if (statusPagamento == StatusPagamento.Aprovado)
		{
			AtualizarEstoque(pedido);
			pedido.Status = StatusPedido.Processando;
		}
		else
		{
			pedido.Status = StatusPedido.Cancelado;
		}

		await _pedidoWriteOnly.AtualizarStatus(pedido.Id, pedido.Status);
		await _unitOfWork.Commit();
	}

	private static void AtualizarEstoque(Dominio.Entidades.Pedido pedido)
	{
		foreach (ItemPedido item in pedido.Itens)
		{
			Produto produto = item.Produto
				?? throw new InvalidOperationException($"Produto nulo em ItemPedido ID {item.Id}");

			if (produto.QuantidadeEstoque < item.Quantidade)
				throw new InvalidOperationException($"Estoque insuficiente para '{produto.Nome}' após commit. Pedido {pedido.Id}");

			produto.QuantidadeEstoque -= item.Quantidade;
		}
	}
}