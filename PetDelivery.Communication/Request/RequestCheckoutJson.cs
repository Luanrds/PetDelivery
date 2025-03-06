namespace PetDelivery.Communication.Request;
public class RequestCheckoutJson
{
	public long UsuarioId { get; set; }
	public EnderecoRequestJson Endereco { get; set; } = new();
	public string FormaPagamento { get; set; } = string.Empty;
	public List<ItemCarrinhoDto> ItensCarrinho { get; set; } = [];
}
