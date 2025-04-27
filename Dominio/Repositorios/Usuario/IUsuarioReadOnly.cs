namespace Dominio.Repositorios.Usuario;

public interface IUsuarioReadOnly
{
	Task<bool> ExisteUsuarioAtivoComEmail(string email);
	Task<bool> ExisteUsuarioAtivoComIdentificador(Guid identificadorUsuario);
	Task<Entidades.Usuario?> GetByEmailESenha(string email, string senha);
	Task<Entidades.Usuario?> GetByEmail(string email);
}
