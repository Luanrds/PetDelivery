using Dominio.Repositorios;

namespace Infrastucture.Configuracao;
public class UnitOfWork : IUnitOfWork
{
	private readonly PetDeliveyContext _dbContext;

	public UnitOfWork(PetDeliveyContext dbContext) => _dbContext = dbContext;

	public async Task Commit() => await _dbContext.SaveChangesAsync();
}
