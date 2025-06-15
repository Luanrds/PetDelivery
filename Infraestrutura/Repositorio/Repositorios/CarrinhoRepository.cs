using Dominio.Entidades;
using Dominio.Repositorios.Carrinho;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;

public class CarrinhoRepository(PetDeliveryDbContext dbContext) : ICarrinhoReadOnly, ICarrinhoWriteOnly
{
	public async Task Add(CarrinhoDeCompras carrinho) => await dbContext.CarrinhoDeCompras.AddAsync(carrinho);

	public async Task LimparItensAsync(long carrinhoId)
	{
		List<ItemCarrinhoDeCompra> itensParaRemover = await dbContext.ItemCarrinhoDeCompra
									 .Where(i => i.CarrinhoId == carrinhoId)
									 .ToListAsync();

		if (itensParaRemover.Count != 0)
		{
			dbContext.ItemCarrinhoDeCompra.RemoveRange(itensParaRemover);
		}
	}

	public async Task RemoverItemCarrinho(long itemCarrinhoId)
	{
		ItemCarrinhoDeCompra? item = await dbContext.ItemCarrinhoDeCompra.FindAsync(itemCarrinhoId);

		if (item != null)
		{
			dbContext.ItemCarrinhoDeCompra.Remove(item);
		}
	}

	public async Task<CarrinhoDeCompras?> ObtenhaCarrinhoAtivo(long usuarioId) =>
		await dbContext.CarrinhoDeCompras
			.Include(c => c.ItensCarrinho)
				.ThenInclude(i => i.Produto)
				.ThenInclude(p => p.Usuario)
			.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);

	public void AtualizarItem(ItemCarrinhoDeCompra item) =>
		dbContext.ItemCarrinhoDeCompra.Update(item);
}