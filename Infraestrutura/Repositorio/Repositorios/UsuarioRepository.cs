using Dominio.Entidades;
using Dominio.Repositorios.Usuario;
using Infraestrutura.Configuracao;

namespace Infraestrutura.Repositorio.Repositorios;

public class UsuarioRepository : IUsuarioWriteOnlyRepository
{
	private readonly PetDeliveryDbContext _dbContext;

	public UsuarioRepository(PetDeliveryDbContext dbContext) => _dbContext = dbContext;

	public async Task Add(Usuario usuario) => await _dbContext.Usuario.AddAsync(usuario);
}
