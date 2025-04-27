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

	public async Task<ResponseCarrinhoDeComprasJson> ExecuteAsync(long itemCarrinhoId, RequestAtualizarItemCarrinhoJson request)
	{
		//Validate(request);

		CarrinhoDeCompras? carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(request.UsuarioId)
			?? throw new NotFoundException($"Carrinho não encontrado para o usuário.");

		ItemCarrinhoDeCompra? item = carrinho.ItensCarrinho.FirstOrDefault(i => i.Id == itemCarrinhoId)
			?? throw new NotFoundException($"Item de carrinho com ID {itemCarrinhoId} não encontrado no carrinho do usuário.");

		if (request.Quantidade <= 0)
		{
			await _carrinhoWriteOnly.RemoverItemCarrinho(item.Id);
		}
		else
		{
			Produto? produto = item.Produto;

			if (request.Quantidade > produto.QuantidadeEstoque)
			{
				throw new ErrorOnValidationException([$"Estoque insuficiente para '{produto.Nome}'. Disponível: {produto.QuantidadeEstoque}, Solicitado: {request.Quantidade}."]);
			}

			item.Quantidade = request.Quantidade;
			_carrinhoWriteOnly.AtualizarItem(item);
		}

		await _unitOfWork.Commit();

		CarrinhoDeCompras? carrinhoAtualizado =
			await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(request.UsuarioId);

		return _mapper.Map<ResponseCarrinhoDeComprasJson>(carrinhoAtualizado);
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
