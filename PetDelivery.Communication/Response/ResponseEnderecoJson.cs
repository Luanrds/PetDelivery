namespace PetDelivery.Communication.Response;

public class ResponseEnderecoJson
{
	public long Id { get; set; }
	public long UsuarioId { get; set; }
	public string Rua { get; set; } = string.Empty;
	public string Bairro { get; set; } = string.Empty;
	public string Numero { get; set; } = string.Empty;
	public string Cidade { get; set; } = string.Empty;
	public string Estado { get; set; } = string.Empty;
	public string CEP { get; set; } = string.Empty;
}
