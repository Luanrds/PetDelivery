namespace Dominio.Repositorios.Pedido;
public interface IPedidoReadOnly
{
	Task<Entidades.Pedido?> GetByIdAsync(long pedidoId);
	Task<IList<Entidades.Pedido>> GetByUsuarioIdAsync(long usuarioId);
}
