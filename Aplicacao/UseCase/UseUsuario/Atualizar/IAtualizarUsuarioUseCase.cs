using PetDelivery.Communication.Request;

namespace Aplicacao.UseCase.UseUsuario.Atualizar;
public interface IAtualizarUsuarioUseCase
{
	Task ExecuteAsync(RequestAtualizarUsuarioJson request);
}
