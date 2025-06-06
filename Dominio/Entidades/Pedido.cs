using Dominio.Enums;

namespace Dominio.Entidades;
public class Pedido : EntidadeBase
{
    public long UsuarioId { get; set; }
    public long EnderecoId { get; set; }
    public StatusPedido Status { get; set; } 
    public DateTime DataPedido { get; set; }
    public decimal ValorTotal { get; set; } 
    public virtual List<ItemPedido> Itens { get; set; } = [];
    public virtual Usuario Usuario { get; set; } = null!;
    public virtual Endereco Endereco { get; set; } = null!;
    public virtual Pagamento? Pagamento { get; set; }
}