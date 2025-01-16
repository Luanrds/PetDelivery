using Dominio.Entidades;

namespace Dominio.Repositorios.Carrinho;
public interface ICarrinhoWriteOnly
{
	Task Add(CarrinhoDeCompras carrinho);
}
