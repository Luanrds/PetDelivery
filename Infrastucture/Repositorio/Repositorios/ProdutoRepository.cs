using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using Infrastucture.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Repositorio.Repositorios;

public class ProdutoRepository : IProdutoWriteOnly, IProdutoReadOnly
{
    private readonly PetDeliveryDbContext _dbContext;

    public ProdutoRepository(PetDeliveryDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Produto produto) => await _dbContext.Produto.AddAsync(produto);

	public Task<Produto?> GetById(long ProdutoId)
	{
		return _dbContext.Produto
			.AsNoTracking()
			.FirstOrDefaultAsync(produto => produto.Id == ProdutoId);
	}
}
