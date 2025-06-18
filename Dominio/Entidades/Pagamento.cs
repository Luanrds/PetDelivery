using Dominio.Enums;

namespace Dominio.Entidades;

public class Pagamento : EntidadeBase
{
	public long PedidoId { get; set; }
	public MetodoPagamento MetodoPagamento { get; set; }
	public StatusPagamento StatusPagamento { get; set; }
	public decimal Valor { get; set; }
	public DateTime DataPagamento { get; set; }

	public virtual Pedido Pedido { get; set; } = null!;
}