using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UserUseCase;
public class RegistroDeUsuarioUseCase : IRegistroDeUsuarioUseCase
{
	public Task<ResponseUsuarioRegistradoJson> Execute(RequestRegistroDeUsuarioJson request)
	{
		// validar Request

		//mapear a request em uma entidade

		//Criptografia de senha

		//Salvar no banco de dados

		return new ResponseUsuarioRegistradoJson
		{
			Nome = request.Nome
		};
	}
}
