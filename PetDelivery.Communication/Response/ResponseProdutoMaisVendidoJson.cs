namespace PetDelivery.Communication.Response;
public class ResponseProdutoMaisVendidoJson
{
	public string NomeProduto { get; set; } = string.Empty;
	public string Categoria { get; set; } = string.Empty;
	public int QuantidadeVendas { get; set; }
}
