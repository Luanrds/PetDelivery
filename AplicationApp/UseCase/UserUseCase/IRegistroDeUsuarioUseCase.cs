using PetDelivery.Communication.Response;
using PetDelivery.Communication.Request;

namespace Aplicacao.UseCase.UserUseCase;
public interface IRegistroDeUsuarioUseCase
{
	public Task<ResponseUsuarioRegistradoJson> Execute(RequestRegistroDeUsuarioJson request);
}
