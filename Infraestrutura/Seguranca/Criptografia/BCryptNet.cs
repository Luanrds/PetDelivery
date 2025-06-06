using Dominio.Seguranca.Criptografia;

namespace Infraestrutura.Seguranca.Criptografia;
public class BCryptNet : ISenhaEncripter
{
	public string Encrypt(string password)
	{
		return BCrypt.Net.BCrypt.HashPassword(password);
	}

	public bool IsValid(string password, string passwordHash)
	{
		return BCrypt.Net.BCrypt.Verify(password, passwordHash);
	}
}
