using Dominio.Enums;

namespace PetDelivery.Communication.Request;

public class RequestProdutoJson
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; } = decimal.Zero;
	public CategoriaProduto Categoria { get; set; }
	public int QuantidadeEstoque { get; set; }
}
