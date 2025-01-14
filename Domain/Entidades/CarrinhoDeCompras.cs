namespace Dominio.Entidades;
public class CarrinhoDeCompras : EntidadeBase
{
    public DateTime DataCriacao { get; set; }
    public List<ItemCarrinhoDeCompra> ItensCarrinho { get; set; } = [];
}
