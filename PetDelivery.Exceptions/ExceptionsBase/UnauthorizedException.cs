using System.Net;

namespace PetDelivery.Exceptions.ExceptionsBase;
public class UnauthorizedException(string mensagem) : PetDeliveryExceptions(mensagem)
{
	public override IList<string> GetMensagensDeErro() => [Message];

	public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}