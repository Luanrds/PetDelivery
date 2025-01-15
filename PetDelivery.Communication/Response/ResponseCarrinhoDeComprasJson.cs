namespace PetDelivery.Communication.Response;
public class ResponseCarrinhoDeComprasJson
{
	public long Id { get; set; }
	public long ProdutoId { get; set; }
	public string NomeProduto { get; set; } = string.Empty;
	public decimal PrecoUnitario { get; set; }
	public int Quantidade { get; set; }
	public decimal SubTotal { get; set; }
	public ResponseProdutoJson Produto { get; set; } = new();
}
