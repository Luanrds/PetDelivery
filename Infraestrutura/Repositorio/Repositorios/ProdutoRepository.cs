using Dominio.Entidades;
using Dominio.Enums;
using Dominio.ObjetosDeValor;
using Dominio.Repositorios.Produto;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;

public class ProdutoRepository(PetDeliveryDbContext dbContext) : IProdutoWriteOnly, IProdutoReadOnly, IProdutoUpdateOnly
{
	public async Task Add(Produto produto) => await dbContext.Produto.AddAsync(produto);

	public void Atualize(Produto produto) => dbContext.Produto.Update(produto);

	public Task<List<Produto>> GetAll() => dbContext.Produto.Include(p => p.Usuario).ToListAsync();

	public async Task Excluir(long produtoId)
	{
		var produto = await dbContext.Produto.FindAsync(produtoId);
		if (produto != null)
			dbContext.Produto.Remove(produto);
	}

	public Task<Produto?> GetById(long ProdutoId) =>
		dbContext.Produto
			.AsNoTracking()
			.Include(p => p.Usuario)
			.FirstOrDefaultAsync(produto => produto.Id == ProdutoId);

	public async Task<IEnumerable<Produto>> ObterPorCategoria(string categoria) =>
		Enum.TryParse<CategoriaProduto>(categoria, true, out var categoriaEnum)
			? await dbContext.Produto
				.AsNoTracking()
				.Where(produto => produto.Categoria == categoriaEnum)
				.ToListAsync()
			: (IEnumerable<Produto>)([]);

	public async Task<List<Produto>> GetByUsuarioIdAsync(long usuarioId) =>
		await dbContext.Produto
			.AsNoTracking()
			.Include(p => p.Usuario)
			.Where(p => p.UsuarioId == usuarioId)
			.ToListAsync();

	public async Task<int> GetTotalEstoquePorVendedorAsync(long vendedorId) =>
		await dbContext.Produto
			.AsNoTracking()
			.Where(p => p.UsuarioId == vendedorId)
			.SumAsync(p => p.QuantidadeEstoque);

	public async Task<IList<ProdutoVendidoInfo>> GetProdutosMaisVendidosPorVendedorAsync(long vendedorId, int topN = 5)
	{

		var produtoIds = (await dbContext.ItemPedido
			.AsNoTracking()
			.Where(ip => dbContext.Produto.Any(p => p.Id == ip.ProdutoId && p.UsuarioId == vendedorId) &&
						dbContext.Pedido.Any(ped => ped.Id == ip.PedidoId &&
							dbContext.Pagamento.Any(pag => pag.PedidoId == ped.Id &&
								pag.StatusPagamento == StatusPagamento.Aprovado)))
			.Select(ip => new
			{
				ip.ProdutoId,
				ip.Quantidade
			})
			.ToListAsync()).Select(x => x.ProdutoId).Distinct().ToList();
		var produtos = await dbContext.Produto
			.AsNoTracking()
			.Where(p => produtoIds.Contains(p.Id))
			.Select(p => new
			{
				p.Id,
				p.Nome,
				p.Categoria
			})
			.ToListAsync();

		var resultado = (await dbContext.ItemPedido
			.AsNoTracking()
			.Where(ip => dbContext.Produto.Any(p => p.Id == ip.ProdutoId && p.UsuarioId == vendedorId) &&
						dbContext.Pedido.Any(ped => ped.Id == ip.PedidoId &&
							dbContext.Pagamento.Any(pag => pag.PedidoId == ped.Id &&
								pag.StatusPagamento == StatusPagamento.Aprovado)))
			.Select(ip => new
			{
				ip.ProdutoId,
				ip.Quantidade
			})
			.ToListAsync())
			.GroupBy(x => x.ProdutoId)
			.Select(g =>
			{
				var produto = produtos.First(p => p.Id == g.Key);
				return new ProdutoVendidoInfo(
					g.Key,
					produto.Nome,
					produto.Categoria,
					g.Sum(x => x.Quantidade)
				);
			})
			.OrderByDescending(x => x.QuantidadeVendas)
			.Take(topN)
			.ToList();

		return resultado;
	}

	public async Task<(IList<Produto> Produtos, int TotalItens)> Buscar(BuscaProdutosCriteria criteria)
	{
		IQueryable<Produto> query = dbContext.Produto.AsNoTracking().Include(p => p.Usuario).Where(p => p.Ativo);

		if (!string.IsNullOrWhiteSpace(criteria.Termo))
		{
			query = query.Where(p =>
				EF.Functions.ILike(PetDeliveryDbContext.Unaccent(p.Nome), "%" + PetDeliveryDbContext.Unaccent(criteria.Termo) + "%") ||
				EF.Functions.ILike(PetDeliveryDbContext.Unaccent(p.Descricao), "%" + PetDeliveryDbContext.Unaccent(criteria.Termo) + "%")
			);
		}

		if (criteria.PrecoMin.HasValue)
		{
			query = query.Where(p => p.Valor >= criteria.PrecoMin.Value);
		}

		if (criteria.PrecoMax.HasValue)
		{
			query = query.Where(p => p.Valor <= criteria.PrecoMax.Value);
		}

		query = criteria.OrdenarPor?.ToLowerInvariant() switch
		{
			"precoasc" => query.OrderBy(p => p.Valor),
			"precodesc" => query.OrderByDescending(p => p.Valor),
			"nome" => query.OrderBy(p => p.Nome),
			_ => query.OrderBy(p => p.Nome)
		};

		int totalItens = await query.CountAsync();

		List<Produto> produtos = await query
			.Skip((criteria.Pagina - 1) * criteria.ItensPorPagina)
			.Take(criteria.ItensPorPagina)
			.ToListAsync();

		return (produtos, totalItens);
	}
}
