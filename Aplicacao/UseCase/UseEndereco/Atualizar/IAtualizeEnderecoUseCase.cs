using PetDelivery.Communication.Request;

namespace Aplicacao.UseCase.UseEndereco.Atualizar;
public interface IAtualizeEnderecoUseCase
{
	Task ExecuteAsync(long id, RequestAtualizarEnderecoJson request);
}
