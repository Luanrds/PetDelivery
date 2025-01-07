namespace PetDelivery.Exceptions.ExceptionsBase;

public class NotFoundException : PetDeliveryExceptions
{
	public IList<string> MensagensDeErro { get; set; }

	public NotFoundException(IList<string> mensagensDeErro)
    {
		MensagensDeErro = mensagensDeErro;
    }
}
