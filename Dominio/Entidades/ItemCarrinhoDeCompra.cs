namespace Dominio.Entidades;

public class ItemCarrinhoDeCompra : EntidadeBase
{
	public long CarrinhoId { get; set; }
	public long ProdutoId { get; set; }
	public int Quantidade { get; set; }
	public virtual Produto Produto { get; set; } = null!;
	public decimal PrecoUnitario { get; set; }

	public decimal CalcularSubTotal()
	{
		if (Produto == null)
		{
			return 0;
		}
		return Quantidade * Produto.Valor;
	}
}
