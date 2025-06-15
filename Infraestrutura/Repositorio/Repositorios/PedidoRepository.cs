using Dominio.Entidades;
using Dominio.ObjetosDeValor;
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
				.ThenInclude(i => i.Produto)
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

	public async Task<decimal> GetTotalVendasDeHojePorVendedorAsync(long vendedorId)
	{
		var hoje = DateTime.UtcNow.Date;
		var amanha = hoje.AddDays(1);

		return await dbContext.Pedido
			.Where(p => p.DataPedido >= hoje && p.DataPedido < amanha &&
						 p.Pagamento != null &&
						 p.Pagamento.StatusPagamento == StatusPagamento.Aprovado &&
						 p.Itens.Any(item =>
							 item.Produto != null &&
							 item.Produto.UsuarioId == vendedorId))
			.SumAsync(p => p.ValorTotal);
	}

	public async Task<decimal> GetTotalVendasDeOntemPorVendedorAsync(long vendedorId)
	{
		var ontem = DateTime.UtcNow.Date.AddDays(-1);
		var hoje = DateTime.UtcNow.Date;

		return await dbContext.Pedido
			.Where(p => p.DataPedido >= ontem && p.DataPedido < hoje &&
						 p.Pagamento != null &&
						 p.Pagamento.StatusPagamento == StatusPagamento.Aprovado &&
						 p.Itens.Any(item =>
							 item.Produto != null &&
							 item.Produto.UsuarioId == vendedorId))
			.SumAsync(p => p.ValorTotal);
	}

	public async Task<int> GetContagemNovosPedidosDeHojePorVendedorAsync(long vendedorId)
	{
		var hoje = DateTime.UtcNow.Date;
		var amanha = hoje.AddDays(1);

		return await dbContext.Pedido
			.AsNoTracking()
			.Where(p => p.DataPedido >= hoje && p.DataPedido < amanha &&
						 p.Pagamento != null && p.Pagamento.StatusPagamento == StatusPagamento.Aprovado &&
						 p.Itens.Any(item => item.Produto != null && item.Produto.UsuarioId == vendedorId))
			.CountAsync();
	}
	public async Task<int> GetContagemNovosPedidosDeOntemPorVendedorAsync(long vendedorId)
	{
		var ontem = DateTime.UtcNow.Date.AddDays(-1);
		var hoje = DateTime.UtcNow.Date;

		return await dbContext.Pedido
			.AsNoTracking()
			.Where(p => p.DataPedido >= ontem && p.DataPedido < hoje &&
						 p.Pagamento != null && p.Pagamento.StatusPagamento == StatusPagamento.Aprovado &&
						 p.Itens.Any(item => item.Produto != null && item.Produto.UsuarioId == vendedorId))
			.CountAsync();
	}

	public async Task<IList<Pedido>> GetUltimosPedidosContendoProdutosDoVendedorAsync(long vendedorId, int topN = 5)
	{
		return await dbContext.Pedido
			.AsNoTracking()
			.Include(p => p.Usuario)
			.Include(p => p.Itens)
				.ThenInclude(i => i.Produto)
			.Where(p => p.Itens.Any(item => item.Produto != null && item.Produto.UsuarioId == vendedorId))
			.OrderByDescending(p => p.DataPedido)
			.Take(topN)
			.ToListAsync();
	}

	public async Task<IList<VendaMensalInfo>> GetVendasMensaisPorVendedorAsync(long vendedorId, DateTime dataInicioUtc, DateTime dataFimUtc)
	{
		DateTime dataFimExclusive = dataFimUtc.Date.AddDays(1);

		return await dbContext.Pedido
			.AsNoTracking()
			.Where(p => p.DataPedido >= dataInicioUtc && p.DataPedido < dataFimExclusive &&
						 p.Pagamento != null &&
						 p.Pagamento.StatusPagamento == StatusPagamento.Aprovado &&
						 p.Itens.Any(item => item.Produto != null && item.Produto.UsuarioId == vendedorId))
			.GroupBy(p => new { p.DataPedido.Year, p.DataPedido.Month })
			.Select(g => new VendaMensalInfo
			{
				Ano = g.Key.Year,
				Mes = g.Key.Month,
				TotalVendas = g.Sum(p => p.ValorTotal)
			})
			.OrderBy(vm => vm.Ano)
			.ThenBy(vm => vm.Mes)
			.ToListAsync();
	}

	public async Task<bool> ProdutoJaVendido(long produtoId) =>
		await dbContext.ItemPedido.AnyAsync(item => item.ProdutoId == produtoId);
}
