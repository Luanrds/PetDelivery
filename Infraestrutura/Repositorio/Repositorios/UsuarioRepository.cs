using Dominio.Entidades;
using Dominio.Repositorios.Usuario;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;

public class UsuarioRepository : IUsuarioWriteOnly, IUsuarioReadOnly
{
	private readonly PetDeliveryDbContext _dbContext;

	public UsuarioRepository(PetDeliveryDbContext dbContext) => _dbContext = dbContext;

	public async Task Add(Usuario usuario) => await _dbContext.Usuario.AddAsync(usuario);

	public void Atualize(Usuario usuario) => _dbContext.Usuario.Update(usuario);

	public Task<List<Usuario>> GetAll()
	{
		throw new NotImplementedException();
	}

	public Task<Usuario> GetByEmail(string email)
	{
		throw new NotImplementedException();
	}

	public async Task<Usuario?> GetById(long id) => await _dbContext.Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
	
}
