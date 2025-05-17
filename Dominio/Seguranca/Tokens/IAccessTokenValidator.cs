namespace Dominio.Seguranca.Tokens;
public interface IAccessTokenValidator
{
	public Guid ValidarEBuscarIdentificadorDoUsuario(string token);
}
