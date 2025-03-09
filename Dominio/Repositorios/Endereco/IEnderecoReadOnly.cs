namespace Dominio.Repositorios.Endereco;
public interface IEnderecoReadOnly
{
	Task<IEnumerable<Dominio.Entidades.Endereco>> GetByUsuarioId(long usuarioId);
}
