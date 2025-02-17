using Dominio.Enums;

namespace Dominio.Entidades;

public class Pagamento : EntidadeBase
{
	public long PedidoId {  get; set; }
	public MetodoPagamento Metodo { get; set; } = new();
	public string Status { get; set; } = string.Empty;
	public decimal Valor { get; set; }
	public DateTime DataPagamento { get; set; } = DateTime.UtcNow;
}