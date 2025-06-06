using System.Net;

namespace PetDelivery.Exceptions.ExceptionsBase;
public class ForbiddenException(string message) : PetDeliveryExceptions(message)
{
	public override IList<string> GetMensagensDeErro() => [Message];

	public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}
