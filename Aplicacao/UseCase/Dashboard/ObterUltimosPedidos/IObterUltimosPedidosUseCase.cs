using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Dashboard.ObterUltimosPedidos;
public interface IObterUltimosPedidosUseCase
{
	Task<ResponseUltimosPedidosJson> ExecuteAsync(int topN = 5);
}
