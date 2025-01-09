using System.Net;

namespace PetDelivery.Exceptions.ExceptionsBase;

public class NotFoundException(string menssagem) : PetDeliveryExceptions(menssagem)
{
	public override IList<string> GetMensagensDeErro() => [Message];

	public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
