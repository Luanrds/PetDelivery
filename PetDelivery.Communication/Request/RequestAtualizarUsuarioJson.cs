namespace PetDelivery.Communication.Request;

public class RequestAtualizarUsuarioJson
{
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	//public DateOnly? DataNascimento { get; set; }
}
