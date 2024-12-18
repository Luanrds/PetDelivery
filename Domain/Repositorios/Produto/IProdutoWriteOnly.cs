namespace Dominio.Repositorios.Produto;

public interface IProdutoWriteOnly
{
    public Task Add(Entidades.Produto produto);
}
