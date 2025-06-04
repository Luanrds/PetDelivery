namespace PetDelivery.Communication.Response;
public class ResponseProdutosMaisVendidosJson
{
	public IList<ResponseProdutoMaisVendidoJson> Produtos { get; set; } = new List<ResponseProdutoMaisVendidoJson>();
}
