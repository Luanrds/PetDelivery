using PetDelivery.Communication.Response;
using PetDelivery.Communication.Request;

namespace Aplicacao.UseCase.UseUsuario.Criar;
public interface IRegistroDeUsuarioUseCase
{
	public Task<ResponseUsuarioRegistradoJson> Execute(RequestRegistroDeUsuarioJson request);
}
