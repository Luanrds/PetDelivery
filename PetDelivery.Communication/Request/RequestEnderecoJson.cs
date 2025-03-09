namespace PetDelivery.Communication.Request;
public class RequestEnderecoJson
{
	public long UsuarioId { get; set; }
	public string Rua { get; set; } = string.Empty;
	public string Numero { get; set; } = string.Empty;
	public string Bairro { get; set; } = string.Empty;
	public string Cidade { get; set; } = string.Empty;
	public string Estado { get; set; } = string.Empty;
	public string CEP { get; set; } = string.Empty;
}
