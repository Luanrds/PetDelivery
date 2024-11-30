using Dominio.Interfaces.InterfaceProducts;
using Dominio.Interfaces.InterfaceServices;
using Entidades.Entidades;

namespace Dominio.Services;
public class ServicoDoProduto(IProduct IProduto) : IServiceProduct
{
    private readonly IProduct _IProduto = IProduto;

    public async Task AddProduct(Produto produto)
    {
        bool validaNome = produto.ValidarPropriedadeString(produto.Nome, "Nome");

        bool validaValor = produto.ValidarPropriedadeDecimal(produto.Valor, "Valor");

        if (validaNome && validaValor)
        {
            produto.Estado = true;
            await _IProduto.Add(produto);
        }
    }

    public async Task UpdateProduct(Produto produto)
    {
        bool validaNome = produto.ValidarPropriedadeString(produto.Nome, "Nome");

        bool validaValor = produto.ValidarPropriedadeDecimal(produto.Valor, "Valor");

        if (validaNome && validaValor)
        {
            await _IProduto.Update(produto);
        }
    }
}