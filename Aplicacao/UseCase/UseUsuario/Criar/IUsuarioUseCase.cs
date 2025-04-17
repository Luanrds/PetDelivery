using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.Criar;

public interface IUsuarioUseCase
{
	Task<ResponseUsuarioJson> ExecuteAsync(RequestUsuarioRegistroJson request);
}