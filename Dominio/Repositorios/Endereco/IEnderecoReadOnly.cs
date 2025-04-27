namespace Dominio.Repositorios.Endereco;
public interface IEnderecoReadOnly
{
	Task<Entidades.Endereco?> GetById(long usuarioId, long enderecoId);

	Task<IEnumerable<Dominio.Entidades.Endereco>> GetByUsuarioId(long usuarioId);
}
