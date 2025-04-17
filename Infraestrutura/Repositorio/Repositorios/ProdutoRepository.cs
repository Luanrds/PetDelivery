using Dominio.Entidades;
using Dominio.Enums;
using Dominio.Repositorios.Produto;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;

public class ProdutoRepository : IProdutoWriteOnly, IProdutoReadOnly, IProdutoUpdateOnly
{
    private readonly PetDeliveryDbContext _dbContext;

    public ProdutoRepository(PetDeliveryDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Produto produto) => await _dbContext.Produto.AddAsync(produto);

	public void Atualize(Produto produto) => _dbContext.Produto.Update(produto);

	public Task<List<Produto>> GetAll() => _dbContext.Produto.ToListAsync();

	public async Task Excluir(long produtoId)
	{
		var produto = await _dbContext.Produto.FindAsync(produtoId);

		_dbContext.Produto.Remove(produto!);
	}


	public Task<Produto?> GetById(long ProdutoId)
	{
		return _dbContext.Produto
			.AsNoTracking()
			.FirstOrDefaultAsync(produto => produto.Id == ProdutoId);
	}

	public async Task<IEnumerable<Produto>> ObterPorCategoria(string categoria) =>
		Enum.TryParse<CategoriaProduto>(categoria, true, out var categoriaEnum)
			? await _dbContext.Produto
				.AsNoTracking()
				.Where(produto => produto.Categoria == categoriaEnum)
				.ToListAsync()
			: (IEnumerable<Produto>)([]);
}
