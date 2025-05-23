using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.Login;
public interface ILoginUseCase
{
	Task<ResponseUsuarioJson?> ExecuteAsync(RequestLoginUsuarioJson request);
}
