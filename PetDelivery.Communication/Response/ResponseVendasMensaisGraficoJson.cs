namespace PetDelivery.Communication.Response;
public class ResponseVendasMensaisGraficoJson
{
	public IList<string> Categorias { get; set; } = [];
	public IList<ResponseVendasMensaisDadosSerieJson> Series { get; set; } = [];
}
