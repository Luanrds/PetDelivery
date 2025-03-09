
namespace Dominio.Repositorios.Usuario;
public interface IUsuarioWriteOnly
{
	public Task Add(Entidades.Usuario usuario);
	void Atualize(Entidades.Usuario produto);
	public Task Excluir(long usuarioId);
}
