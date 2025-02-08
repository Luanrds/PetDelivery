using Dominio.Entidades;
using Dominio.Repositorios.Carrinho;
using Infrastucture.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Repositorio.Repositorios;

public class CarrinhoRepository : ICarrinhoReadOnly, ICarrinhoWriteOnly
{
	private readonly PetDeliveryDbContext _dbContext;

	public CarrinhoRepository(PetDeliveryDbContext dbContext) => _dbContext = dbContext;

	public async Task Add(CarrinhoDeCompras carrinho) =>
		await _dbContext.CarrinhoDeCompras.AddAsync(carrinho);

	public Task<CarrinhoDeCompras?> ObtenhaCarrinhoAtivo()
	{
		return _dbContext.CarrinhoDeCompras
			.Include(c => c.ItensCarrinho)
			.OrderByDescending(c => c.Id)
			.FirstOrDefaultAsync();
	}

    public async Task<ItemCarrinhoDeCompra?> ObterItemCarrinhoPorId(long itemId)
    {
		return await _dbContext.ItemCarrinhoDeCompra
			.Include(i => i.Carrinho)
			.FirstOrDefaultAsync(item => item.Id == itemId);
    }

	public async Task LimparCarrinho(CarrinhoDeCompras carrinho)
	{
		_dbContext.ItemCarrinhoDeCompra.RemoveRange(carrinho.ItensCarrinho);
		await _dbContext.SaveChangesAsync();
	}

	public async Task RemoverItemCarrinho(long itemId)
	{
		var item = await _dbContext.ItemCarrinhoDeCompra.FindAsync(itemId);

		if (item is not null)
		{
			_dbContext.ItemCarrinhoDeCompra.Remove(item);
		}
	}

	public async Task Excluir(long produtoId)
    {
        var produto = await _dbContext.Produto.FindAsync(produtoId);

        _dbContext.Produto.Remove(produto!);
    }
}