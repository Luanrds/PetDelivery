namespace PetDelivery.Communication.Request;
public class RequestBuscaProdutosJson
{
	public string? Termo { get; set; }
	public decimal? PrecoMin { get; set; }
	public decimal? PrecoMax { get; set; }
	public string? OrdenarPor { get; set; }
	public int Pagina { get; set; } = 1;
	public int ItensPorPagina { get; set; } = 10;
}
