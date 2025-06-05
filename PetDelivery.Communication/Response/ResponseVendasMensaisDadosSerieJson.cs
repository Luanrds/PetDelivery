namespace PetDelivery.Communication.Response
{
	public class ResponseVendasMensaisDadosSerieJson
	{
	    public string Nome { get; set; } = string.Empty;
	    public IList<decimal> Dados { get; set; } = [];
	}
}
