namespace PetDelivery.Communication.Response;
public class ResponseVendasHojeJson
{
	public decimal ValorVendasHoje { get; set; }
	public decimal? VariacaoOntemPercentual { get; set; }
	public string MensagemComparacao { get; set; } = string.Empty;
}
