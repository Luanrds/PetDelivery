namespace PetDelivery.Communication.Response;
public class ResponsePedidoJson
{
	public long PedidoId { get; set; }
	public string Status { get; set; } = string.Empty;
	public decimal ValorTotal { get; set; }
	public DateTime DataPedido { get; set; }
}
