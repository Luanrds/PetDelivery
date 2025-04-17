using Dominio.Enums;
using System.Text.Json.Serialization;

namespace PetDelivery.Communication.Response;

public class ResponseProdutoJson
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; } = decimal.Zero;
	public string Categoria { get; set; } = string.Empty;
	public int QuantidadeEstoque { get; set; }
}
