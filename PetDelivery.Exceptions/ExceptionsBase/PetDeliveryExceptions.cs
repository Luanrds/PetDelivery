using System.Net;

namespace PetDelivery.Exceptions.ExceptionsBase;
public abstract class PetDeliveryExceptions(string message) : SystemException(message)
{
	public abstract IList<string> GetMensagensDeErro();
	public abstract HttpStatusCode GetStatusCode();

}
