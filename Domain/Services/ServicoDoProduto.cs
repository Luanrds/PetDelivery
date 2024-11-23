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
	}

	public async Task AddProduto(Produto produto)
	{
        bool validaNome = produto.ValidarPropriedadeString(produto.Nome, "Nome");

        bool validaValor = produto.ValidarPropriedadeDecimal(produto.Valor, "Valor");

		if(validaNome && validaValor)
		{
			produto.Estado = true;
			await _IProduto.Add(produto);
		}
    }

	public async Task UpdateProtudo(Produto produto)
	{
        bool validaNome = produto.ValidarPropriedadeString(produto.Nome, "Nome");

        bool validaValor = produto.ValidarPropriedadeDecimal(produto.Valor, "Valor");

        if (validaNome && validaValor)
        {
            await _IProduto.Update(produto);
        }
    }
}
