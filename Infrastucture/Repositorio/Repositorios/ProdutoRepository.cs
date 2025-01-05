using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using Infrastucture.Configuracao;

namespace Infrastucture.Repositorio.Repositorios;

public class ProdutoRepository : IProdutoWriteOnly, IProdutoReadOnly
{
    private readonly PetDeliveryDbContext _dbContext;

    public ProdutoRepository(PetDeliveryDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Produto produto) => await _dbContext.Produto.AddAsync(produto);
}
