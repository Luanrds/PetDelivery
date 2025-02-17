using Dominio.Enums;

namespace Dominio.Entidades;
public class Pedido : EntidadeBase
{
    public long UserId { get; set; }
	public Endereco EnderecoEntrega { get; set; } = new();
	public Pagamento Pagamento { get; set; } = new();
	public StatusPedido Status { get; set; }
	public DateTime DataPedido { get; set; }
    public List<ItemPedido> ItensPedido { get; set; } = [];
}
