namespace Aplicacao.UseCase.Carrinho.RemoverItem;
public interface IRemoveItemCarrinhoUseCase
{
	Task ExecuteAsync(long itemId);
}
