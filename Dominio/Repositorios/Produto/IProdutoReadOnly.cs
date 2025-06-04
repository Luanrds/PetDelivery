namespace Dominio.Repositorios.Produto;

public interface IProdutoReadOnly
{
	Task<Entidades.Produto?> GetById(long ProdutoId);
	Task<List<Entidades.Produto>> GetAll();
	Task<IEnumerable<Entidades.Produto>> ObterPorCategoria(string categoria);
	Task<List<Entidades.Produto>> GetByUsuarioIdAsync(long usuarioId);
	Task<int> GetTotalEstoquePorVendedorAsync(long vendedorId);
}
