namespace PetDelivery.Communication.Response;
public class ResponseItemPedidoJson
{
	public long Id { get; set; }
	public long ProdutoId { get; set; }
	public string NomeProduto { get; set; } = string.Empty;
	public int Quantidade { get; set; }
	public decimal PrecoUnitario { get; set; }
	public decimal SubTotal { get; set; }
}
