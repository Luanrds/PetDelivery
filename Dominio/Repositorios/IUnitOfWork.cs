namespace Dominio.Repositorios;
public interface IUnitOfWork
{
	public Task Commit();
}
