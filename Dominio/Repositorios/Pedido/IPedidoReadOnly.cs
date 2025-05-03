namespace Dominio.Repositorios.Pedido;
public interface IPedidoReadOnly
{
	Task<Entidades.Pedido?> GetByIdAsync(long pedidoId);
	Task<IList<Entidades.Pedido>> GetByUsuarioIdAsync(long usuarioId);
	Task<List<long>> ObterIdsDePedidosComPagamentoPendenteAsync(int maximoPedidos, CancellationToken cancellationToken);
	Task<List<long>> ObterIdsDePedidosParaProcessarEntregaAsync(int maximoPedidos, CancellationToken cancellationToken);
}
