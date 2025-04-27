namespace Dominio.Seguranca.Criptografia;
public interface ISenhaEncripter
{
	public string Encrypt(string password);
	public bool IsValid(string password, string passwordHash);
}
