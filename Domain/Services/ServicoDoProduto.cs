using Dominio.Interfaces.InterfaceProducts;
using Dominio.Interfaces.InterfaceServices;
using Entidades.Entidades;

namespace Dominio.Services;
public class ServicoDoProduto : IServicoDoProduto
{
	private readonly IProduto _IProduto;

    public ServicoDoProduto(IProduto IProduto)
	{
		_IProduto = IProduto;
		var teste = 0;
	}

	public Task AddProduto(Produto produto)
	{
		throw new NotImplementedException();
	}

	public Task UpdateProtudo(Produto produto)
	{
		throw new NotImplementedException();
	}
}
