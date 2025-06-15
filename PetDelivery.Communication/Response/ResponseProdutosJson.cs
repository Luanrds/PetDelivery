namespace PetDelivery.Communication.Response;
public class ResponseProdutosJson
{
	public IList<ResponseProdutoJson> Produtos { get; set; } = [];
	public int Total { get; set; }
	public int PaginaAtual { get; set; }
	public int TotalPaginas { get; set; }
}
