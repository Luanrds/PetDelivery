using Dominio.Enums;
using System.Text.Json.Serialization;

namespace PetDelivery.Communication.Response;

public class ResponseProdutoJson
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Valor { get; set; } = decimal.Zero;
    public bool Disponivel { get; set; } = false;
    public string Descricao { get; set; } = string.Empty;
	public string CategoriaProduto { get; set; } = string.Empty;
}
