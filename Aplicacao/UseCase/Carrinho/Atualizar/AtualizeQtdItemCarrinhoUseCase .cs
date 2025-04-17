using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios.Produto;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.Carrinho.Atualizar;

public class AtualizeQtdItemCarrinhoUseCase : IAtualizeQtdItemCarrinhoUseCase
{
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly ICarrinhoWriteOnly _carrinhoWriteOnly;
	private readonly IProdutoReadOnly _produtoReadOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public AtualizeQtdItemCarrinhoUseCase(
		ICarrinhoReadOnly carrinhoReadOnly,
		ICarrinhoWriteOnly carrinhoWriteOnly,
		IProdutoReadOnly produtoReadOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper)
	{
		_carrinhoReadOnly = carrinhoReadOnly;
		_carrinhoWriteOnly = carrinhoWriteOnly;
		_produtoReadOnly = produtoReadOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<ResponseCarrinhoDeComprasJson> ExecuteAsync(long itemId, RequestAtualizarItemCarrinhoJson request)
	{
		//Validate(request);

		ItemCarrinhoDeCompra item = await _carrinhoReadOnly.ObterItemCarrinhoPorId(itemId, request.UsuarioId)
			?? throw new NotFoundException($"Item de carrinho com ID {itemId} não encontrado para o usuário.");

		if (request.Quantidade <= 0)
		{
			await _carrinhoWriteOnly.RemoverItemCarrinho(item.Id, request.UsuarioId);
		}
		else
		{
			Produto? produto = await _produtoReadOnly.GetById(item.ProdutoId);

			if (produto is null)
			{
				await _carrinhoWriteOnly.RemoverItemCarrinho(item.Id, request.UsuarioId);
				throw new NotFoundException($"Produto associado ao item (ID: {item.ProdutoId}) não encontrado.");
			}

			if (request.Quantidade > produto.QuantidadeEstoque)
			{
				throw new ErrorOnValidationException([$"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.QuantidadeEstoque}, Solicitado: {request.Quantidade}."]);
			}

			item.Quantidade = request.Quantidade;
		}

		await _unitOfWork.Commit();

		CarrinhoDeCompras? carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(request.UsuarioId);

		return _mapper.Map<ResponseCarrinhoDeComprasJson>(carrinho);
	}

	//private static void Validate(RequestAtualizarItemCarrinhoJson request)
	//{
	//	var validator = new CarrinhoValidator();

	//	var result = validator.Validate(request);

	//	if (result.IsValid == false)
	//	{
	//		var mensagensDeErro = result.Errors.Select(e => e.ErrorMessage).ToList();

	//		throw new ErrorOnValidationException(mensagensDeErro);
	//	}
	//}
}
