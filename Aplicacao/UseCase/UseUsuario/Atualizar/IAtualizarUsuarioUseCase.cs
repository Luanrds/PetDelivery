using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.Atualizar;
public interface IAtualizarUsuarioUseCase
{
	Task Execute(long id, RequestAtualizarUsuarioJson request);
}
