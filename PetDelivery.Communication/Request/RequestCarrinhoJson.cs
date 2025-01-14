namespace PetDelivery.Communication.Request;
public class RequestCarrinhoJson
{
	public DateTime DataCriacao { get; set; }
	public List<ItemCarrinhoDeCompra> ItensCarrinho { get; set; } = [];
}
