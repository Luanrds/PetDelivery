using Entidades.Entidades;
using FluentValidation.Results;

namespace Dominio.Interfaces.InterfaceServices;
public interface IServiceProduct
{
    Task<ValidationResult> AddProduct(Produto produto);

    Task UpdateProduct(Produto produto);
}
