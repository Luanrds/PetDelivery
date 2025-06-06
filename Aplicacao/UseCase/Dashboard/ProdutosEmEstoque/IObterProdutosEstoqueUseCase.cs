using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Dashboard.ProdutosEmEstoque;
public interface IObterProdutosEstoqueUseCase
{
	Task<ResponseProdutosEstoqueJson> ExecuteAsync();
}
