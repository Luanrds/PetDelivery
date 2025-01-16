namespace Dominio.Entidades;
public class ItemCarrinhoDeCompra : EntidadeBase
{
	public long CarrinhoId { get; set; }
	public long ProdutoId { get; set; }
	public int Quantidade { get; set; }
	public decimal PrecoUnitario { get; set; }
	public CarrinhoDeCompras Carrinho { get; set; } = new();
	public Produto Produto { get; set; } = new();

	public decimal CalcularSubTotal()
	{
		return Quantidade * PrecoUnitario;
	}
}
