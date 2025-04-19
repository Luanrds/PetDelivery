using PetDelivery.Communication.Request;

namespace Aplicacao.UseCase.UseEndereco.Atualizar;
public interface IAtualizeEnderecoUseCase
{
	Task ExecuteAsync(long usuarioId, long endedrecoId, RequestAtualizarEnderecoJson request);
}
