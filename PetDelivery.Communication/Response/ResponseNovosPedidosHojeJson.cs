namespace PetDelivery.Communication.Response;
public class ResponseNovosPedidosHojeJson
{
	public int QuantidadeNovosPedidos { get; set; }
	public decimal? VariacaoOntemPercentual { get; set; }
	public string MensagemComparacao { get; set; } = string.Empty;
}
