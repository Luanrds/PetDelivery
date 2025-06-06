namespace PetDelivery.Communication.Response;

public class ResponseUsuarioJson
{
	public string Nome { get; set; } = string.Empty;
	public bool EhVendedor { get; set; } = false;
	public ResponseTokensJson Tokens { get; set; } = default!;
}