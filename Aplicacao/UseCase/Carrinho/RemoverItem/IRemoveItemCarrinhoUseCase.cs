using Dominio.Entidades;

namespace Aplicacao.UseCase.Carrinho.RemoverItem;
public interface IRemoveItemCarrinhoUseCase
{
	Task ExecuteRemover(long itemId, long usuarioId);
}
