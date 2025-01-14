namespace Dominio.Entidades;
public class Pedido : EntidadeBase
{
    public DateTime DataPedido { get; set; }
    public List<ItemPedido> Itens { get; set; } = [];
}
