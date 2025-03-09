namespace Dominio.Repositorios.Endereco;
public interface IEnderecoReadOnly
{
	Task<Entidades.Endereco?> GetById(long ProdutoId);
	Task<IEnumerable<Dominio.Entidades.Endereco>> GetByUsuarioId(long usuarioId);
}
