using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using Infrastucture.Configuracao;

namespace Infrastucture.Repositorio.Repositorios;

public class ProdutoRepository : IProdutoWriteOnly, IProdutoReadOnly
{
    private readonly PetDeliveyContext _dbContext;

    public ProdutoRepository(PetDeliveyContext dbContext) => _dbContext = dbContext;

    public async Task Add(Produto produto) => await _dbContext.AddAsync(produto);
}
