using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Dashboard.VendasHoje;
public interface IObterVendasHojeUseCase
{
	Task<ResponseVendasHojeJson> ExecuteAsync();
}
