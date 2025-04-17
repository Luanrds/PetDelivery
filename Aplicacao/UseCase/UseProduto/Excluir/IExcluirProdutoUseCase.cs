namespace Aplicacao.UseCase.UseProduto.Excluir;
public interface IExcluirProdutoUseCase
{
	Task ExecuteAsync(long produtoId);
}
