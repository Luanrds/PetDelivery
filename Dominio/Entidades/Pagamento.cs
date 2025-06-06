using Dominio.Enums;

namespace Dominio.Entidades;

public class Pagamento : EntidadeBase
{
	public long PedidoId { get; set; }
	public MetodoPagamento MetodoPagamento { get; set; } = new();
	public StatusPagamento StatusPagamento { get; set; } = new();
	public decimal Valor { get; set; }
	public DateTime DataPagamento { get; set; }

	public Pedido Pedido { get; set; } = new();
}