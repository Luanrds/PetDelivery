namespace Aplicacao.UseCase.UseProduto.Excluir;
public interface IExcluirProdutoUseCase
{
	Task Execute(long produtoId);
}
