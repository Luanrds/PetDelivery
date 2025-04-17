using Aplicacao.Validadores;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios.Produto;
using Dominio.Repositorios.Usuario;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.Carrinho.Criar;

public class CarrinhoUseCase : ICarrinhoUseCase
{
	private readonly ICarrinhoWriteOnly _carrinhoWriteOnly;
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly IProdutoReadOnly _produtoReadOnly;
	private readonly IUsuarioReadOnly _usuarioReadOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public CarrinhoUseCase(
		ICarrinhoWriteOnly carrinhoWriteOnly,
		ICarrinhoReadOnly carrinhoReadOnly,
		IProdutoReadOnly produtoReadOnly,
		IUsuarioReadOnly usuarioReadOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper)
	{
		_carrinhoWriteOnly = carrinhoWriteOnly;
		_carrinhoReadOnly = carrinhoReadOnly;
		_produtoReadOnly = produtoReadOnly;
		_usuarioReadOnly = usuarioReadOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<ResponseCarrinhoDeComprasJson> ExecuteAsync(RequestItemCarrinhoJson request)
	{
		Validate(request);

		Usuario usuario = await _usuarioReadOnly.GetById(request.UsuarioId)
			?? throw new NotFoundException($"Usuário com ID {request.UsuarioId} não encontrado.");

		CarrinhoDeCompras carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(request.UsuarioId)
			?? new CarrinhoDeCompras
				{
					UsuarioId = request.UsuarioId,
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
				PrecoUnitario = produto.Valor,
			};
			carrinho.ItensCarrinho.Add(novoItem);
		}

		if (carrinho.Id == 0)
		{
			await _carrinhoWriteOnly.Add(carrinho);
		}

		await _unitOfWork.Commit();

		CarrinhoDeCompras? carrinhoAtualizado = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(request.UsuarioId);

		return _mapper.Map<ResponseCarrinhoDeComprasJson>(carrinhoAtualizado);
	}

	private static void Validate(RequestItemCarrinhoJson request)
	{
		var validator = new CarrinhoValidator();

		var result = validator.Validate(request);

		if (result.IsValid == false)
		{
			var mensagensDeErro = result.Errors.Select(e => e.ErrorMessage).ToList();

			throw new ErrorOnValidationException(mensagensDeErro);
		}
	}
}