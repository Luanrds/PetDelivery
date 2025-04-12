namespace Dominio.Repositorios.Usuario;
public interface IUsuarioReadOnly
{	Task<Entidades.Usuario?> GetById(long id);
	Task<Entidades.Usuario> GetByEmail(string email);
	Task<List<Entidades.Usuario>> GetAll();
}
