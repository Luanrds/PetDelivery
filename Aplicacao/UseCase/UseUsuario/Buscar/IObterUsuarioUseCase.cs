using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.Buscar;
public interface IObterUsuarioUseCase
{
	Task<ResponseUsuarioJson> Execute(long id);
}
