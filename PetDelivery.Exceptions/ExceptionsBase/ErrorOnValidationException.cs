namespace PetDelivery.Exceptions.ExceptionsBase;

public class ErrorOnValidationException : PetDeliveryExceptions
{
    public IList<string> MensagensDeErro { get; set; }

    public ErrorOnValidationException(IList<string> mensagensDeErro)
    {
        MensagensDeErro = mensagensDeErro;
    }
}
