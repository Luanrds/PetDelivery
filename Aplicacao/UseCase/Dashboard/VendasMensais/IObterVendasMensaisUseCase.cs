using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Dashboard.VendasMensais;
public interface IObterVendasMensaisUseCase
{
	Task<ResponseVendasMensaisGraficoJson> ExecuteAsync(string periodo);
}
