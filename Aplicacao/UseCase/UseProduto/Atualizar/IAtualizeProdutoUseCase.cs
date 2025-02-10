using PetDelivery.Communication.Request;

namespace Aplicacao.UseCase.UseProduto.Atualizar;
public interface IAtualizeProdutoUseCase
{
	Task Execute(long produtoId, RequestProdutoJson requisicao);
}
