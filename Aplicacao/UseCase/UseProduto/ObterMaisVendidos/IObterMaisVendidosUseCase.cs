using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.ObterMaisVendidos;
public interface IObterMaisVendidosUseCase
{
	Task<ResponseProdutosJson> ExecuteAsync(int limite);
}