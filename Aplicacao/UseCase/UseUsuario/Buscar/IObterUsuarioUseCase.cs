using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.Buscar;
public interface IObterUsuarioUseCase
{
	Task<ResponseUsuarioJson> ExecuteAsync(long id);
}
