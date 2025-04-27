using Dominio.Entidades;
using Dominio.Repositorios.Usuario;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;

public class UsuarioRepository(PetDeliveryDbContext dbContext) : IUsuarioWriteOnly, IUsuarioReadOnly, IUsuarioUpdateOnly
{
	private readonly PetDeliveryDbContext _dbContext = dbContext;

	public async Task Add(Usuario usuario) => await _dbContext.Usuario.AddAsync(usuario);

	public void Atualize(Usuario usuario) => _dbContext.Usuario.Update(usuario);

	public async Task Excluir(long usuarioId)
	{
		Usuario? usuario = await _dbContext.Usuario.FindAsync(usuarioId);

		_dbContext.Usuario.Remove(usuario!);
	}

	public async Task<Usuario> GetById(long id) =>
		await _dbContext.Usuario.FirstAsync(u => u.Id == id);

	public async Task<Usuario?> GetByEmail(string email) =>
	await _dbContext.Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.Ativo && u.Email.Equals(email));

	public async Task<bool> ExisteUsuarioAtivoComEmail(string email) =>
		await _dbContext.Usuario.AnyAsync(u => u.Email.Equals(email) && u.Ativo);

	public async Task<bool> ExisteUsuarioAtivoComIdentificador(Guid identificadorUsuario) =>
		await _dbContext.Usuario.AnyAsync(u => u.IdentificadorDoUsuario.Equals(identificadorUsuario) && u.Ativo);

	public Task<Usuario?> GetByEmailESenha(string email, string senha) =>
		_dbContext.Usuario.AsNoTracking().FirstOrDefaultAsync(u => u.Ativo && u.Email.Equals(email) && u.Senha.Equals(senha));

}
