using Dominio.Enums;

namespace Dominio.Entidades;
public class Pedido : EntidadeBase
{
	public long ClienteId { get; set; }
	public long EnderecoEntregaId { get; set; }
	public long PagamentoId { get; set; }
	public StatusPedido Status { get; set; } = new();
	public DateTime DataPedido { get; set; } = new();
	public List<ItemPedido> ItensPedido { get; set; } = [];

	public Usuario Usuario { get; set; } = new();
	public Endereco EnderecoEntrega { get; set; } = new();
	public Pagamento Pagamento { get; set; } = new();
}
