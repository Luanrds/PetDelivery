namespace Dominio.Repositorios.Pedido;
public interface IPedidoWriteOnly
{
	Task Adicionar(Entidades.Pedido pedido);
	Task AtualizarStatus(long pedidoId, Enums.StatusPedido status);
}
