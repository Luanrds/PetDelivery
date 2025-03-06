using Dominio.Enums;

namespace Dominio.Entidades;

public class Pagamento : EntidadeBase
{
	public long PedidoId { get; set; }
	public MetodoPagamento Metodo { get; set; } = new();
	public StatusPagamento Status { get; set; } = new();
	public decimal Valor { get; set; }
	public DateTime DataPagamento { get; set; } = DateTime.UtcNow;

	public Pedido Pedido { get; set; } = new();
}