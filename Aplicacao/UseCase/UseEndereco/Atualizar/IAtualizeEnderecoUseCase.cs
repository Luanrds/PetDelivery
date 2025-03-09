using PetDelivery.Communication.Request;

namespace Aplicacao.UseCase.UseEndereco.Atualizar;
public interface IAtualizeEnderecoUseCase
{
	Task Execute(long id, RequestAtualizarEnderecoJson request);
}
