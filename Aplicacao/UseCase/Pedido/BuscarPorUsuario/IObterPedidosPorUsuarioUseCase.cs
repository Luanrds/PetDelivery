using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Pedido.BuscarPorUsuario;
public interface IObterPedidosPorUsuarioUseCase
{
	Task<ResponseListaPedidosJson> Execute();
}
