using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Carrinho.Criar;
public interface ICarrinhoUseCase
{
    Task<ResponseCarrinhoDeComprasJson> ExecuteAsync(RequestItemCarrinhoJson request);
}
