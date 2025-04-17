using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.Criar;
public interface IProdutoUseCase
{
	public Task<ResponseProdutoJson> ExecuteAsync(RequestProdutoJson request);
}
