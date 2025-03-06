using Dominio.Entidades;
using Dominio.Repositorios.Carrinho;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;

public class CarrinhoRepository : ICarrinhoReadOnly, ICarrinhoWriteOnly
{
	private readonly PetDeliveryDbContext _dbContext;

	public CarrinhoRepository(PetDeliveryDbContext dbContext) => _dbContext = dbContext;

	public async Task Add(CarrinhoDeCompras carrinho) =>
		await _dbContext.CarrinhoDeCompras.AddAsync(carrinho);

	public async Task<CarrinhoDeCompras?> ObtenhaCarrinhoAtivo(long usuarioId)
	{
		return await _dbContext.CarrinhoDeCompras
			.Include(c => c.ItensCarrinho)
			.Include(c => c.Usuario)
			.Where(c => c.Usuario.Id == usuarioId)
			.OrderByDescending(c => c.Id)
			.FirstOrDefaultAsync();
	}

	public async Task<ItemCarrinhoDeCompra?> ObterItemCarrinhoPorId(long itemId, long usuarioId)
	{
		return await _dbContext.ItemCarrinhoDeCompra
			.Include(i => i.Carrinho)
			.Include(i => i.Carrinho.Usuario)
			.Where(i => i.Id == itemId && i.Carrinho.Usuario.Id == usuarioId)
			.FirstOrDefaultAsync();
	}

	public async Task LimparCarrinho(CarrinhoDeCompras carrinho)
	{
		_dbContext.ItemCarrinhoDeCompra.RemoveRange(carrinho.ItensCarrinho);
		await _dbContext.SaveChangesAsync();
	}

	public async Task RemoverItemCarrinho(long itemId, long usuarioId)
	{
		var item = await _dbContext.ItemCarrinhoDeCompra
			.Include(i => i.Carrinho)
			.Include(i => i.Carrinho.Usuario)
			.Where(i => i.Id == itemId && i.Carrinho.Usuario.Id == usuarioId)
			.FirstOrDefaultAsync();

		if (item != null)
		{
			_dbContext.ItemCarrinhoDeCompra.Remove(item);
			await _dbContext.SaveChangesAsync();
		}
	}
}