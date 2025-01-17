namespace PetDelivery.Communication.Response;

public class ResponseItemCarrinhoJson
{
	public long Id { get; set; }
	public ResponseProdutoJson Produto { get; set; } = new();
	public int Quantidade { get; set; }
	public decimal PrecoUnitario { get; set; }
	public decimal SubTotal { get; set; }
}