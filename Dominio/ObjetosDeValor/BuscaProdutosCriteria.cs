namespace Dominio.ObjetosDeValor;
public class BuscaProdutosCriteria
{
	public string? Termo { get; init; }
	public decimal? PrecoMin { get; init; }
	public decimal? PrecoMax { get; init; }
	public string? OrdenarPor { get; init; }
	public int Pagina { get; init; } = 1;
	public int ItensPorPagina { get; init; } = 10;
}
