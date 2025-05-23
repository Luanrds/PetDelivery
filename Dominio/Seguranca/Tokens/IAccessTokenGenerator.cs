namespace Dominio.Seguranca.Tokens;
public interface IAccessTokenGenerator
{
	public string Gererate(Guid IdentificadorDoUsuario);
}
