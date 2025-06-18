namespace PetDelivery.Communication.Request;

public class RequestUsuarioRegistroJson
{
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Senha { get; set; } = string.Empty;
	public string Telefone { get; set; } = string.Empty;
	public bool EhVendedor { get; set; } = false;
}
