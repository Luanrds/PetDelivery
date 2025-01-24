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
}