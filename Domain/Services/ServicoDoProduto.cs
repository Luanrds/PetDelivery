using Dominio.Interfaces.InterfaceProducts;
using Dominio.Interfaces.InterfaceServices;
using Dominio.Validadores;
using Entidades.Entidades;
using FluentValidation.Results;

namespace Dominio.Services;
public class ServicoDoProduto(IProduct IProduto) : IServiceProduct
{
    private readonly IProduct _IProduto = IProduto;

    public async Task AddProduct(Produto produto)
    {
		var validationResult = Validate(produto);

		if (validationResult.IsValid)
        {
            produto.Estado = true;
            await _IProduto.Add(produto);
        }
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