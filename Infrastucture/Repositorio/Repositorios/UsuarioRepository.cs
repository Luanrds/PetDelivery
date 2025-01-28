using Dominio.Entidades;
using Dominio.Repositorios.Usuario;
using Infrastucture.Configuracao;

namespace Infrastucture.Repositorio.Repositorios;
public class UsuarioRepository : IUsuarioReadOnlyRepository, IUsuarioWriteOnlyRepository
{
	private readonly PetDeliveryDbContext _dbContext;

	public UsuarioRepository(PetDeliveryDbContext dbContext) => _dbContext = dbContext;

	public async Task Add(Usuario usuario) => await _dbContext.Usuario.AddAsync(usuario);
}
