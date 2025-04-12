using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.Criar;

public interface IUsuarioUseCase
{
	Task<ResponseUsuarioJson> Execute(RequestUsuarioRegistroJson request);
}