using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Dashboard.ProdutosMaisVendidos;
public interface IObterProdutosMaisVendidosUseCase
{
	Task<ResponseProdutosMaisVendidosJson> ExecuteAsync(int topN = 5);
}
