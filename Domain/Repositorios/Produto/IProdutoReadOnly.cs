namespace Dominio.Repositorios.Produto;

public interface IProdutoReadOnly
{
	Task<Entidades.Produto?> GetById(long ProdutoId);
}
