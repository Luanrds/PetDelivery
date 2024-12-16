using Dominio.Repositorios;

namespace Infrastucture.Configuracao;
public class UnitOfWork : IUnitOfWork
{
	private readonly PetDeliveyContext _dbContext;

	public UnitOfWork(PetDeliveyContext dbContext) => _dbContext = dbContext;

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
