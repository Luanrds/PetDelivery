namespace Dominio.Entidades;

public class CarrinhoDeCompras : EntidadeBase
{
	//public long UsuarioId { get; set; }
	public List<ItemCarrinhoDeCompra> ItensCarrinho { get; set; } = [];
	public Usuario Usuario { get; set; } = new();
}
