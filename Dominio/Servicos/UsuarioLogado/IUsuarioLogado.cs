using Dominio.Entidades;

namespace Dominio.Servicos.UsuarioLogado;
public interface IUsuarioLogado
{
	public Task<Usuario> Usuario();
}
