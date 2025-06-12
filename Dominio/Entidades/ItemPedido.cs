namespace Dominio.Entidades;

public class ItemPedido : EntidadeBase
{
	public long PedidoId { get; set; }
	public long ProdutoId { get; set; }
	public Produto Produto { get; set; } = null!;
	public decimal PrecoUnitario { get; set; }
	public int Quantidade { get; set; }
	public decimal SubTotal => Quantidade * PrecoUnitario;

	public Pedido Pedido { get; set; } = null!;
}