using Dominio.Repositorios;

namespace Infraestrutura.Configuracao;
public class UnitOfWork : IUnitOfWork
{
	private readonly PetDeliveryDbContext _dbContext;

	public UnitOfWork(PetDeliveryDbContext dbContext) => _dbContext = dbContext;

    public async Task Commit()
    {
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Erro ao salvar alterações no banco: {ex.Message}", ex);
        }
    }
}
