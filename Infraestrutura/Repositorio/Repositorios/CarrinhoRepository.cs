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
			.Where(c => c.UsuarioId == usuarioId)
			.OrderByDescending(c => c.Id)
			.FirstOrDefaultAsync();
	}

	public async Task<ItemCarrinhoDeCompra?> ObterItemCarrinhoPorId(long itemId, long usuarioId)
	{
		return await _dbContext.ItemCarrinhoDeCompra
			.Where(i => i.Id == itemId && _dbContext.CarrinhoDeCompras.Any(c => c.Id == i.CarrinhoId && c.UsuarioId == usuarioId))
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
			.Where(i => i.Id == itemId && _dbContext.CarrinhoDeCompras.Any(c => c.Id == i.CarrinhoId && c.UsuarioId == usuarioId))
			.FirstOrDefaultAsync();

		if (item != null)
		{
			_dbContext.ItemCarrinhoDeCompra.Remove(item);
			await _dbContext.SaveChangesAsync();
		}
	}
}