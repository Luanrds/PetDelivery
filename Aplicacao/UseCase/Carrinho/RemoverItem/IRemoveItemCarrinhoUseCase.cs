using Dominio.Entidades;

namespace Aplicacao.UseCase.Carrinho.RemoverItem;
public interface IRemoveItemCarrinhoUseCase
{
	Task ExecuteAsync(long itemId, long usuarioId);
}
