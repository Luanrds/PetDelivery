using Dominio.Entidades;
using Dominio.Enums;
using Dominio.Repositorios.Produto;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;

public class ProdutoRepository(PetDeliveryDbContext dbContext) : IProdutoWriteOnly, IProdutoReadOnly, IProdutoUpdateOnly
{
	public async Task Add(Produto produto) => await dbContext.Produto.AddAsync(produto);

	public void Atualize(Produto produto) => dbContext.Produto.Update(produto);

	public Task<List<Produto>> GetAll() => dbContext.Produto.ToListAsync();

	public async Task Excluir(long produtoId)
	{
		var produto = await dbContext.Produto.FindAsync(produtoId);

		dbContext.Produto.Remove(produto!);
	}

	public Task<Produto?> GetById(long ProdutoId) => 
		dbContext.Produto
		.AsNoTracking()
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
		.Where(p => p.UsuarioId == usuarioId)
		.ToListAsync();
	
}
