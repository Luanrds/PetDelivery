using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios;
using PetDelivery.Exceptions.ExceptionsBase;

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

	public async Task ExecuteRemover(long itemId, long usuarioId)
	{
		var item = await _carrinhoReadOnly.ObterItemCarrinhoPorId(itemId, usuarioId);

		if (item == null)
		{
			throw new NotFoundException("Item não encontrado.");
		}

		await _carrinhoWriteOnly.RemoverItemCarrinho(item.Id, usuarioId);
		await _unitOfWork.Commit();
	}
}