namespace PetDelivery.Communication.Response;
public class ResponseCarrinhoDeComprasJson
{
	public long Id { get; set; }
	public ResponseProdutoJson Produto { get; set; } = new();
	public int Quantidade { get; set; }
	public decimal SubTotal { get; set; }
}
