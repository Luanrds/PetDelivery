using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Dashboard.NovosPedidosHoje;
public interface IObterNovosPedidosHojeUseCase
{
	Task<ResponseNovosPedidosHojeJson> ExecuteAsync();
}
