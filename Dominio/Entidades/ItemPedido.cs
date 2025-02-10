namespace Dominio.Entidades;
public class ItemPedido : EntidadeBase
{
	public long PedidoId { get; set; }
	public long ProdutoId { get; set; }
	public int Quantidade { get; set; }
	public decimal PrecoUnitario { get; set; }
	public Pedido Pedido { get; set; } = new Pedido();
	public Produto Produto { get; set; } = new Produto();
}
