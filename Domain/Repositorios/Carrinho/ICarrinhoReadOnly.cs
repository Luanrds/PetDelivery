using Dominio.Entidades;

namespace Dominio.Repositorios.Carrinho;
public interface ICarrinhoReadOnly
{
	Task<CarrinhoDeCompras?> ObtenhaCarrinhoAtivo();
}
