using Entidades.Entidades;

namespace Aplicacao.Interfaces
{
    public interface InterfaceProductApp : InterfaceGenericaApp<Produto>
    {
        Task AddProduct(Produto produto);

        Task UpdateProduct(Produto produto);
    }
}
