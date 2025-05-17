using System.Net;

namespace PetDelivery.Exceptions.ExceptionsBase;
public class LoginInvalidoException : PetDeliveryExceptions
{
	public LoginInvalidoException() : base("Email ou senha incorretos") { }

	public override IList<string> GetMensagensDeErro() => [Message];

	public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
