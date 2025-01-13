namespace Dominio.Repositorios.Produto;

public interface IProdutoWriteOnly
{
    public Task Add(Entidades.Produto produto);

    public Task Excluir(long produtoId);
}
