using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios;
using PetDelivery.Exceptions.ExceptionsBase;
using Dominio.Entidades;
using Dominio.Servicos.UsuarioLogado;

namespace Aplicacao.UseCase.Carrinho.RemoverItem;

public class RemoveItemCarrinhoUseCase : IRemoveItemCarrinhoUseCase
{
	private readonly ICarrinhoWriteOnly _carrinhoWriteOnly;
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IUsuarioLogado _usuarioLogado;

	public RemoveItemCarrinhoUseCase(
		ICarrinhoWriteOnly carrinhoWriteOnly,
		ICarrinhoReadOnly carrinhoReadOnly,
		IUnitOfWork unitOfWork,
		IUsuarioLogado usuarioLogado)
	{
		_carrinhoWriteOnly = carrinhoWriteOnly;
		_carrinhoReadOnly = carrinhoReadOnly;
		_unitOfWork = unitOfWork;
		_usuarioLogado = usuarioLogado;
	}

	public async Task ExecuteAsync(long itemCarrinhoId)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		CarrinhoDeCompras? carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(usuarioLogado.Id)
			?? throw new NotFoundException("Carrinho não encontrado para este usuário.");

		ItemCarrinhoDeCompra? item = carrinho.ItensCarrinho.FirstOrDefault(i => i.Id == itemCarrinhoId)
			?? throw new NotFoundException($"Item com ID {itemCarrinhoId} não encontrado no carrinho.");

		await _carrinhoWriteOnly.RemoverItemCarrinho(item.Id);

		await _unitOfWork.Commit();
	}
}