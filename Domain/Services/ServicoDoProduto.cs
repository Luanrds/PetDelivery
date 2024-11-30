using Dominio.Interfaces.InterfaceProducts;
using Dominio.Interfaces.InterfaceServices;
using Dominio.Validadores;
using Entidades.Entidades;

namespace Dominio.Services;
public class ServicoDoProduto(IProduct IProduto) : IServiceProduct
{
    private readonly IProduct _IProduto = IProduto;

    private readonly ProdutoValidator _validator = new();

    public async Task AddProduct(Produto produto)
    {
        var validationResult = _validator.Validate(produto);

        if (validationResult.IsValid)
        {
            produto.Estado = true;
            await _IProduto.Add(produto);
        }
    }

    public async Task UpdateProduct(Produto produto)
    {
        var validationResult = _validator.Validate(produto);

        if (validationResult.IsValid)
        {
            await _IProduto.Update(produto);
        }
    }
}