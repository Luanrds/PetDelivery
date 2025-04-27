using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Pedido.Criar;
public interface ICriarPedidoUseCase
{
	Task<ResponsePedidoCriadoJson> Execute(RequestCheckoutJson request);
}
