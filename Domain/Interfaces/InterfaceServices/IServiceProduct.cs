using Entidades.Entidades;

namespace Dominio.Interfaces.InterfaceServices;
public interface IServiceProduct
{
	Task AddProduct(Produto produto);

	Task UpdateProduct(Produto produto);
}
