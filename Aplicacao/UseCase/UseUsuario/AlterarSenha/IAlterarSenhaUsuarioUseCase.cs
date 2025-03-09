using PetDelivery.Communication;

namespace Aplicacao.UseCase.UseUsuario.AlterarSenha;
public interface IAlterarSenhaUsuarioUseCase
{
	Task Execute(long id, RequestAlterarSenhaUsuarioJson request);
	
}
