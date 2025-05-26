namespace PetDelivery.Communication.Response;

public class ResponseItemCarrinhoJson
{
	public long Id { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Descricao { get; set; } = string.Empty;
	public string? ImagemUrl { get; set; }
	public int Quantidade { get; set; }
	public decimal SubTotal { get; set; }
}