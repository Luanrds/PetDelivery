using AutoMapper;
using Dominio.Entidades;
using Dominio.Enums;
using Dominio.Repositorios;
using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios.Endereco;
using Dominio.Repositorios.Pagamento;
using Dominio.Repositorios.Pedido;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.Pedido.Criar;

public class CriarPedidoUseCase : ICriarPedidoUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly ICarrinhoWriteOnly _carrinhoWriteOnly;
	private readonly IEnderecoReadOnly _enderecoRepository;
	private readonly IPedidoWriteOnly _pedidoRepository;
	private readonly IMetodoPagamentoUsuarioRepository _metodoPagamentoRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public CriarPedidoUseCase(
		IUsuarioLogado usuarioLogado,
		ICarrinhoReadOnly carrinhoReadOnly,
		ICarrinhoWriteOnly carrinhoWriteOnly,
		IEnderecoReadOnly enderecoRepository,
		IPedidoWriteOnly pedidoRepository,
		IMetodoPagamentoUsuarioRepository metodoPagamentoRepository,
		IUnitOfWork unitOfWork,
		IMapper mapper)
	{
		_usuarioLogado = usuarioLogado;
		_carrinhoReadOnly = carrinhoReadOnly;
		_carrinhoWriteOnly = carrinhoWriteOnly;
		_enderecoRepository = enderecoRepository;
		_pedidoRepository = pedidoRepository;
		_metodoPagamentoRepository = metodoPagamentoRepository;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<ResponsePedidoCriadoJson> Execute(RequestCheckoutJson request)
	{
		var usuario = await _usuarioLogado.Usuario();

		_ = await _enderecoRepository.GetById(usuario.Id, request.EnderecoId)
			?? throw new NotFoundException($"Endereço com ID {request.EnderecoId} não foi encontrado ou não pertence a este usuário.");

		var carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(usuario.Id);
		if (carrinho is null || !carrinho.ItensCarrinho.Any())
		{
			throw new ErrorOnValidationException(["Seu carrinho está vazio."]);
		}

		var metodoDePagamentoFinal = await ValidarMetodoPagamento(request, usuario.Id);

		var pedido = new Dominio.Entidades.Pedido
		{
			UsuarioId = usuario.Id,
			EnderecoId = request.EnderecoId,
			Status = StatusPedido.Concluido,
			DataPedido = DateTime.UtcNow,
			Itens = [],
			ValorTotal = 0
		};

		foreach (var itemCarrinho in carrinho.ItensCarrinho)
		{
			if (itemCarrinho.Produto.QuantidadeEstoque < itemCarrinho.Quantidade)
			{
				throw new ErrorOnValidationException([$"Estoque insuficiente para o produto '{itemCarrinho.Produto.Nome}'. Disponível: {itemCarrinho.Produto.QuantidadeEstoque}."]);
			}

			var itemPedido = new ItemPedido
			{
				ProdutoId = itemCarrinho.ProdutoId,
				Quantidade = itemCarrinho.Quantidade,
				PrecoUnitarioOriginal = itemCarrinho.Produto.Valor,
				PrecoUnitarioPago = itemCarrinho.Produto.ObterPrecoFinal(),
				ValorDesconto = itemCarrinho.Produto.ValorDesconto,
				TipoDesconto = itemCarrinho.Produto.TipoDesconto
			};
			pedido.Itens.Add(itemPedido);

			itemCarrinho.Produto.QuantidadeEstoque -= itemCarrinho.Quantidade;
		}
		pedido.ValorTotal = pedido.Itens.Sum(item => item.SubTotal);

		var pagamento = new Dominio.Entidades.Pagamento
		{
			Valor = pedido.ValorTotal,
			MetodoPagamento = metodoDePagamentoFinal,
			StatusPagamento = StatusPagamento.Pendente,
			DataPagamento = DateTime.UtcNow,
		};
		pedido.Pagamento = pagamento;

		await _pedidoRepository.Adicionar(pedido);

		pagamento.StatusPagamento = StatusPagamento.Aprovado;
		pedido.Status = StatusPedido.Processando;

		await _carrinhoWriteOnly.LimparItensAsync(carrinho.Id);

		await _unitOfWork.Commit();

		return _mapper.Map<ResponsePedidoCriadoJson>(pedido);
	}

	private async Task<MetodoPagamento> ValidarMetodoPagamento(RequestCheckoutJson request, long usuarioId)
	{
		if (request.MetodoPagamentoUsuarioId.HasValue)
		{
			var cartaoSalvo = await _metodoPagamentoRepository.ObterPorIdEUsuarioId(request.MetodoPagamentoUsuarioId.Value, usuarioId);

			return cartaoSalvo is null
				? throw new NotFoundException("O método de pagamento selecionado não foi encontrado ou não pertence a este usuário.")
				: cartaoSalvo.Tipo == TipoCartao.Credito ? MetodoPagamento.CartaoCredito : MetodoPagamento.CartaoDebito;
		}

		if (request.MetodoPagamentoAvulso.HasValue)
		{
			var metodoAvulso = request.MetodoPagamentoAvulso.Value;

			return metodoAvulso == MetodoPagamento.PIX || metodoAvulso == MetodoPagamento.Boleto
				? metodoAvulso
				: throw new ErrorOnValidationException(["O método de pagamento avulso selecionado é inválido."]);
		}

		throw new ErrorOnValidationException(["É obrigatório fornecer um método de pagamento para criar o pedido."]);
	}
}