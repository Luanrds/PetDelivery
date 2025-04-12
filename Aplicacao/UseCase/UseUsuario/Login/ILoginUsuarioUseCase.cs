using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.Login;
public interface ILoginUsuarioUseCase
{
	Task<ResponseUsuarioJson?> Execute(RequestLoginUsuarioJson request);
}
