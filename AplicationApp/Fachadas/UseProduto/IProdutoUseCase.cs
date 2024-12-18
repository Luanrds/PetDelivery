using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.Fachadas.UseProduto;
public interface IProdutoUseCase
{
	public Task<ResponseProdutoJson> CrieProduto(RequestProdutoJson request);
}
