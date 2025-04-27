using Dominio.Entidades;

namespace Dominio.Repositorios.Carrinho;

public interface ICarrinhoWriteOnly
{	
	Task Add(CarrinhoDeCompras carrinho);
	Task LimparItensAsync(long carrinhoId);
	Task RemoverItemCarrinho(long itemCarrinhoId);
	void AtualizarItem(ItemCarrinhoDeCompra item);
}