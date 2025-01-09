using System.Net;

namespace PetDelivery.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : PetDeliveryExceptions
{
	private readonly IList<string> _mensagensDeErro;

    public ErrorOnValidationException(IList<string> mensagensDeErro) : base(string.Empty)
    {
		_mensagensDeErro = mensagensDeErro;
    }

	public override IList<string> GetMensagensDeErro() => _mensagensDeErro;

	public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
