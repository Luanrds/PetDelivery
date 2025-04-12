namespace PetDelivery.Communication.Response;

public class ResponseUsuarioJson
{
	public int Id { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public bool EhVendedor { get; set; } = false;

}