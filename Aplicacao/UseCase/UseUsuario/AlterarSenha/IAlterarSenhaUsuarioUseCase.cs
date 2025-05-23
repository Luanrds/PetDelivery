using PetDelivery.Communication;

namespace Aplicacao.UseCase.UseUsuario.AlterarSenha;
public interface IAlterarSenhaUsuarioUseCase
{
	Task ExecuteAsync(RequestAlterarSenhaUsuarioJson request);
	
}
