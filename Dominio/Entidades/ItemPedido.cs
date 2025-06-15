using Dominio.Enums;

namespace Dominio.Entidades;

public class ItemPedido : EntidadeBase
{
	public long PedidoId { get; set; }
	public long ProdutoId { get; set; }
	public Produto Produto { get; set; } = null!;
	public decimal PrecoUnitarioOriginal { get; set; }
	public decimal PrecoUnitarioPago { get; set; }
	public decimal? ValorDesconto { get; set; }
	public TipoDesconto? TipoDesconto { get; set; }

	public int Quantidade { get; set; }
	public decimal SubTotal => Quantidade * PrecoUnitarioPago;

	public Pedido Pedido { get; set; } = null!;
}