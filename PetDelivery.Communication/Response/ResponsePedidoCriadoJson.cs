namespace PetDelivery.Communication.Response;

public class ResponsePedidoCriadoJson
{
	public long PedidoId { get; set; }
	public string StatusInicial { get; set; } = string.Empty;
}
