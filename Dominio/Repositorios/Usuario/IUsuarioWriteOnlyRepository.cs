namespace Dominio.Repositorios.Usuario;
public interface IUsuarioWriteOnlyRepository
{
	public Task Add(Entidades.Usuario usuario);
}
