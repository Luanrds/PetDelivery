using Entidades.Entidades;

namespace Dominio.Interfaces.InterfaceServices;
public interface IServicoDoProduto
{
	Task AddProduto(Produto produto);

	Task UpdateProtudo(Produto produto);
}
