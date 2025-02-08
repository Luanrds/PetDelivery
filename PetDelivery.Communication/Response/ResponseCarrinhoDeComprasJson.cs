namespace PetDelivery.Communication.Response;
public class ResponseCarrinhoDeComprasJson
{
	public long Id { get; set; }
	public List<ResponseItemCarrinhoJson> Itens { get; set; } = [];
	public decimal Total { get; set; }
}