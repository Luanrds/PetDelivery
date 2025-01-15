using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Carrinho;
public interface ICarrinhoUseCase
{
	Task<ResponseCarrinhoDeComprasJson> Execute(RequestItemCarrinhoJson request);
}
