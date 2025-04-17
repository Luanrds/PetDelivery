using PetDelivery.Communication.Request;

namespace Aplicacao.UseCase.UseProduto.Atualizar;
public interface IAtualizeProdutoUseCase
{
	Task ExecuteAsync(long produtoId, RequestProdutoJson requisicao);
}
