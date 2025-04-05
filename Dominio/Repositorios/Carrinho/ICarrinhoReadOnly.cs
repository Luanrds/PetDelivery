using Dominio.Entidades;

namespace Dominio.Repositorios.Carrinho;
public interface ICarrinhoReadOnly
{
	Task<CarrinhoDeCompras?> ObtenhaCarrinhoAtivo(long UsuarioId);
	Task<ItemCarrinhoDeCompra?> ObterItemCarrinhoPorId(long itemId, long usuarioId);
}
