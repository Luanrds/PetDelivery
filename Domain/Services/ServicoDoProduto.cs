using Dominio.Interfaces.InterfaceProducts;
using Dominio.Interfaces.InterfaceServices;
using Dominio.Validadores;
using Entidades.Entidades;
using FluentValidation.Results;

namespace Dominio.Services;
public class ServicoDoProduto : IServiceProduct
{
    private readonly IProduct _IProduto;

    public ServicoDoProduto(IProduct product)
    {
        _IProduto = product;
    }

    public async Task<ValidationResult> AddProduct(Produto produto)
    {
        var validationResult = Validate(produto);

        if (validationResult.IsValid)
        {
            produto.Disponivel = true;
            await _IProduto.Add(produto);
        }

        return validationResult;
    }

    public async Task UpdateProduct(Produto produto)
    {
        var validationResult = Validate(produto);

		if (validationResult.IsValid)
        {
            await _IProduto.Update(produto);
        }
    }

	private static ValidationResult Validate(Produto produto)
	{
        ProdutoValidator validator = new();
		return validator.Validate(produto);
	}
}