namespace Dominio.Repositorios.Usuario;
public interface IUsuarioReadOnlyRepository
{
	public Task<Entidades.Usuario?> GetById(long usuarioId);
}
