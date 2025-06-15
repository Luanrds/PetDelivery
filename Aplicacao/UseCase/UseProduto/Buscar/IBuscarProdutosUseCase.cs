using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.Buscar;
public interface IBuscarProdutosUseCase
{
	Task<ResponseProdutosJson> Execute(RequestBuscaProdutosJson request);
}
