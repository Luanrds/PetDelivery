using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.Buscar;
public interface IObterPerfilUsuarioUseCase
{
	Task<ResponsePerfilUsuarioJson> ExecuteAsync();
}
