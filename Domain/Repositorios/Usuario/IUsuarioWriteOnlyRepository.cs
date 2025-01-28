namespace Dominio.Repositorios.Usuario;
public interface IUsuarioWriteOnlyRepository
{
	public Task Add(Dominio.Entidades.Usuario usuario);
}
