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
	public async Task<List<long>> ObterIdsDePedidosComPagamentoPendenteAsync(int maximoPedidos, CancellationToken cancellationToken)
	{
		return await dbContext.Pedido
			.Include(p => p.Pagamento) // Incluir Pagamento para filtrar pelo status dele
			.Where(p => p.Pagamento != null && p.Pagamento.StatusPagamento == StatusPagamento.Pendente)
			.OrderBy(p => p.DataPedido) // Processar os mais antigos primeiro
			.Select(p => p.Id)          // Selecionar apenas o ID
			.Take(maximoPedidos)        // Limitar a quantidade por ciclo
			.ToListAsync(cancellationToken);
	}

	public async Task<List<long>> ObterIdsDePedidosParaProcessarEntregaAsync(int maximoPedidos, CancellationToken cancellationToken)
	{
		// Pedidos que precisam iniciar a entrega (Processando) ou já estão em trânsito (Enviado)
		var statusParaProcessar = new List<StatusPedido> { StatusPedido.Processando, StatusPedido.Enviado };

		return await dbContext.Pedido
			.Where(p => statusParaProcessar.Contains(p.Status))
			.OrderBy(p => p.DataPedido) // Ou talvez por data de atualização?
			.Select(p => p.Id)
			.Take(maximoPedidos)
			.ToListAsync(cancellationToken);
	}
}
