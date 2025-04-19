using Dominio.Entidades;
using Dominio.Enums;
using Dominio.Repositorios.Pedido;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;
public class PedidoRepository(PetDeliveryDbContext dbContext) : IPedidoReadOnly, IPedidoWriteOnly
{
	public async Task<Pedido?> GetByIdAsync(long pedidoId) => 
		await dbContext.Pedido
			.AsNoTracking()
			.Include(p => p.Itens)
				.ThenInclude(i => i.Produto)
			.Include(p => p.Pagamento)
			.Include(p => p.Endereco)
			.FirstOrDefaultAsync(p => p.Id == pedidoId);

	public async Task<IList<Pedido>> GetByUsuarioIdAsync(long usuarioId) => 
		await dbContext.Pedido
			.AsNoTracking()
			.Where(p => p.UsuarioId == usuarioId)
			.Include(p => p.Itens)
			.Include(p => p.Pagamento)
			.Include(p => p.Endereco)
			.OrderByDescending(p => p.DataPedido)
			.ToListAsync();

	public async Task Adicionar(Pedido pedido) => 
		await dbContext.Pedido.AddAsync(pedido);

	public async Task AtualizarStatus(long pedidoId, StatusPedido status)
	{
		Pedido? pedido = await dbContext.Pedido.FindAsync(pedidoId);
		if (pedido != null)
		{
			pedido.Status = status;
			dbContext.Pedido.Update(pedido);
		}
	}
}
