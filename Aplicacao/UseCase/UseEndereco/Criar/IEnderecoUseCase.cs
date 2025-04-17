using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseEndereco.Criar;
public interface IEnderecoUseCase
{
	Task<ResponseEnderecoJson> ExecuteAsync(RequestEnderecoJson request);
}
