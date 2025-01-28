using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.BuscarPorEmail;
public interface IBucarPerfilUsuarioUseCase
{
	Task<ResponsePerfilUsuario> Execute(long usuarioId);
}
