using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Pedido.ObterPedido;
public interface IObterPedidoPorIdUseCase
{
	Task<ResponsePedidoJson> Execute(long pedidoId);
}
