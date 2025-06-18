namespace PetDelivery.Communication.Response;
public class ResponseItemPedidoJson
{
	public long Id { get; set; }
	public string NomeProduto { get; set; } = string.Empty;
	public string? ImagemUrl { get; set; }
	public decimal PrecoUnitarioOriginal { get; set; }
	public decimal PrecoUnitarioPago { get; set; }
	public decimal? ValorDesconto { get; set; }
	public int? TipoDesconto { get; set; }
	public int Quantidade { get; set; }
	public decimal SubTotal { get; set; }
}
