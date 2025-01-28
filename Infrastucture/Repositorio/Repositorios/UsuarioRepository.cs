using Dominio.Entidades;
using Dominio.Repositorios.Usuario;
using Infrastucture.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Repositorio.Repositorios;
public class UsuarioRepository : IUsuarioReadOnlyRepository, IUsuarioWriteOnlyRepository
{
	private readonly PetDeliveryDbContext _dbContext;

	public UsuarioRepository(PetDeliveryDbContext dbContext) => _dbContext = dbContext;

	public async Task Add(Usuario usuario) => await _dbContext.Usuario.AddAsync(usuario);

	public Task<Usuario?> GetById(long usuarioId)
	{
		return _dbContext.Usuario
			.AsNoTracking()
			.FirstOrDefaultAsync(usuario => usuario.Id == usuarioId);
	}
}
