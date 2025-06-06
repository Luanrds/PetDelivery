using Aplicacao.Validadores;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Repositorios;
using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.UsuarioLogado;
using FluentValidation.Results;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.Carrinho.Criar;

public class CarrinhoUseCase : ICarrinhoUseCase
{
	private readonly ICarrinhoWriteOnly _carrinhoWriteOnly;
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly IProdutoReadOnly _produtoReadOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado;

	public CarrinhoUseCase(
		ICarrinhoWriteOnly carrinhoWriteOnly,
		ICarrinhoReadOnly carrinhoReadOnly,
		IProdutoReadOnly produtoReadOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper,
		IUsuarioLogado usuarioLogado)
	{
		_carrinhoWriteOnly = carrinhoWriteOnly;
		_carrinhoReadOnly = carrinhoReadOnly;
		_produtoReadOnly = produtoReadOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
	}

	public async Task<ResponseCarrinhoDeComprasJson> ExecuteAsync(RequestItemCarrinhoJson request)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		Validate(request);

		CarrinhoDeCompras carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(usuarioLogado.Id)
			?? new CarrinhoDeCompras
			{
				UsuarioId = usuarioLogado.Id,
				ItensCarrinho = []
			};

		Produto produto = await _produtoReadOnly.GetById(request.ProdutoId)
			?? throw new NotFoundException($"Produto com ID {request.ProdutoId} não encontrado.");

		ItemCarrinhoDeCompra? itemExistente = carrinho.ItensCarrinho
			.FirstOrDefault(item => item.ProdutoId == request.ProdutoId);

		int quantidadeFinalNecessaria = (itemExistente?.Quantidade ?? 0) + request.Quantidade;

		if (quantidadeFinalNecessaria > produto.QuantidadeEstoque)
		{
			throw new ErrorOnValidationException([$"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.QuantidadeEstoque}, Solicitado no total: {quantidadeFinalNecessaria}."]);
		}

		if (itemExistente != null)
		{
			itemExistente.Quantidade = quantidadeFinalNecessaria;
		}
		else
		{
			ItemCarrinhoDeCompra novoItem = new()
			{
				ProdutoId = request.ProdutoId,
				Quantidade = request.Quantidade,
			};
			carrinho.ItensCarrinho.Add(novoItem);
		}

		if (carrinho.Id == 0)
		{
			await _carrinhoWriteOnly.Add(carrinho);
		}

		await _unitOfWork.Commit();

		CarrinhoDeCompras? carrinhoAtualizado = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(usuarioLogado.Id);

		return _mapper.Map<ResponseCarrinhoDeComprasJson>(carrinhoAtualizado);
	}

	private static void Validate(RequestItemCarrinhoJson request)
	{
		CarrinhoValidator validator = new();

		ValidationResult result = validator.Validate(request);

		if (result.IsValid.IsFalse())
		{
			List<string> mensagensDeErro = result.Errors.Select(e => e.ErrorMessage).ToList();
			throw new ErrorOnValidationException(mensagensDeErro);
		}
	}
}