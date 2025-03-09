using AutoMapper;
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

	public async Task<ResponseCarrinhoDeComprasJson> Execute(long itemId, RequestAtualizarItemCarrinhoJson request)
	{
		//Validate(request);

		var item = await _carrinhoReadOnly.ObterItemCarrinhoPorId(itemId, request.UsuarioId)
			?? throw new NotFoundException("Item não encontrado.");

		if (request.Quantidade == 0)
		{
			await _carrinhoWriteOnly.RemoverItemCarrinho(item.Id, request.UsuarioId);
		}
		else
		{
			item.Quantidade = request.Quantidade;

			var produto = await _produtoReadOnly.GetById(item.ProdutoId);
			if (produto == null || !produto.Disponivel)
			{
				throw new NotFoundException("Produto não encontrado ou indisponível.");
			}

			item.CalcularSubTotal();
		}

		await _unitOfWork.Commit();

		var carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(request.UsuarioId);

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
