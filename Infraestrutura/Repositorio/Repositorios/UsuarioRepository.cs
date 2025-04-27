using Dominio.Entidades;
using Dominio.Repositorios.Usuario;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;

public class UsuarioRepository(PetDeliveryDbContext dbContext) : IUsuarioWriteOnly, IUsuarioReadOnly
{
	private readonly PetDeliveryDbContext _dbContext = dbContext;

	public async Task Add(Usuario usuario) => await _dbContext.Usuario.AddAsync(usuario);

	public void Atualize(Usuario usuario) => _dbContext.Usuario.Update(usuario);

	public async Task Excluir(long usuarioId)
	{
		Usuario? usuario = await _dbContext.Usuario.FindAsync(usuarioId);

		_dbContext.Usuario.Remove(usuario!);
	}

	public async Task<Usuario?> GetById(long id) => 
		await _dbContext.Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

	public async Task<Usuario?> GetByEmail(string email) =>
	await _dbContext.Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.Ativo && u.Email.Equals(email));

	public async Task<bool> ExisteUsuarioComEmailAtivo(string email) =>
		await _dbContext.Usuario.AnyAsync(u => u.Email.Equals(email) && u.Ativo);

	public Task<List<Usuario>> GetAll()
	{
		throw new NotImplementedException();
	}
}
