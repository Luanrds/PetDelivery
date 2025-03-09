using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Carrinho.Atualizar;

public interface IAtualizeQtdItemCarrinhoUseCase
{
    Task<ResponseCarrinhoDeComprasJson> Execute(long itemId, RequestAtualizarItemCarrinhoJson request);
}
