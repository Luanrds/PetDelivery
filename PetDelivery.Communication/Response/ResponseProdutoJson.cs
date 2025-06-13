using Dominio.Enums;
using System.Text.Json.Serialization;

namespace PetDelivery.Communication.Response;

public class ResponseProdutoJson
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
	public string DescricaoResumida { get; set; } = string.Empty;
	public decimal Valor { get; set; } = decimal.Zero;
	public int Categoria { get; set; }
	public int QuantidadeEstoque { get; set; }
	public string? ImagemUrl { get; set; }
	public List<string>? ImagensUrl { get; set; }
}
