namespace Dominio.Entidades;
public class CarrinhoDeCompras : EntidadeBase
{
    public List<ItemCarrinhoDeCompra> ItensCarrinho { get; set; } = [];
}