using Dominio.Entidades;

namespace Dominio.Repositorios.Carrinho;
public interface ICarrinhoWriteOnly
{
	Task Add(CarrinhoDeCompras carrinho);
	Task LimparCarrinho(CarrinhoDeCompras carrinho);
	Task RemoverItemCarrinho(long item, long usuarioId);
}
