namespace PetDelivery.Communication.Response;
public class ResponseUltimoPedidoJson
{
	public string PedidoId { get; set; } = string.Empty;
	public string NomeCliente { get; set; } = string.Empty;
	public decimal ValorTotal { get; set; }
	public string Status { get; set; } = string.Empty;
}
