using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios;
using PetDelivery.Exceptions.ExceptionsBase;
using Dominio.Entidades;

namespace Aplicacao.UseCase.Carrinho.RemoverItem;

public class RemoveItemCarrinhoUseCase : IRemoveItemCarrinhoUseCase
{
	private readonly ICarrinhoWriteOnly _carrinhoWriteOnly;
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly IUnitOfWork _unitOfWork;

	public RemoveItemCarrinhoUseCase(
		ICarrinhoWriteOnly carrinhoWriteOnly,
		ICarrinhoReadOnly carrinhoReadOnly,
		IUnitOfWork unitOfWork)
	{
		_carrinhoWriteOnly = carrinhoWriteOnly;
		_carrinhoReadOnly = carrinhoReadOnly;
		_unitOfWork = unitOfWork;
	}

	public async Task ExecuteAsync(long itemCarrinhoId, long usuarioId)
	{
		CarrinhoDeCompras carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(usuarioId)
			?? throw new NotFoundException("Carrinho não encontrado para este usuário.");

		ItemCarrinhoDeCompra? item = carrinho.ItensCarrinho.FirstOrDefault(i => i.Id == itemCarrinhoId)
			?? throw new NotFoundException($"Item com ID {itemCarrinhoId} não encontrado no carrinho.");

		await _carrinhoWriteOnly.RemoverItemCarrinho(item.Id); 

		await _unitOfWork.Commit();
	}
}