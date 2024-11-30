using Dominio.Interfaces.InterfaceProducts;
using Dominio.Interfaces.InterfaceServices;
using Dominio.Validadores;
using Entidades.Entidades;
using FluentValidation.Results;

namespace Dominio.Services;
public class ServicoDoProduto(IProduct IProduto) : IServiceProduct
{
    private readonly IProduct _IProduto = IProduto;

    private readonly ProdutoValidator _validator = new();

    public async Task AddProduct(Produto produto)
    {
		ValidationResult result = _validator.Validate(produto);

        if (result.IsValid)
        {
            produto.Estado = true;
            await _IProduto.Add(produto);
        }
    }

    public async Task UpdateProduct(Produto produto)
    {
		ValidationResult result = _validator.Validate(produto);

        if (result.IsValid)
        {
            await _IProduto.Update(produto);
        }
    }
}