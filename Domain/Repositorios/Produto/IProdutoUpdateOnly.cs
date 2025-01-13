namespace Dominio.Repositorios.Produto;
public interface IProdutoUpdateOnly
{
	Task<Entidades.Produto?> GetById(long ProdutoId);

	void Atualize(Entidades.Produto produto);
}
