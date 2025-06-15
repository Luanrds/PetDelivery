namespace PetDelivery.Communication.Response;

public class ResponseProdutoJson
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
	public string DescricaoResumida { get; set; } = string.Empty;
	public decimal ValorOriginal { get; set; }
	public decimal? ValorComDesconto { get; set; }
	public decimal? ValorDesconto { get; set; }
	public int? TipoDesconto { get; set; }
	public int Categoria { get; set; }
	public int QuantidadeEstoque { get; set; }
	public string? ImagemUrl { get; set; }
	public List<string>? ImagensUrl { get; set; }
}
