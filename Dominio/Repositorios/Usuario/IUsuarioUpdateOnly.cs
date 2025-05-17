namespace Dominio.Repositorios.Usuario;
public interface IUsuarioUpdateOnly
{
	Task<Entidades.Usuario> GetById(long id);
	void Atualize(Entidades.Usuario usuario);
}
