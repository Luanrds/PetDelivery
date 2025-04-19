using Dominio.Entidades;
using Dominio.Repositorios.Endereco;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;

public class EnderecoRepository(PetDeliveryDbContext dbContext) : IEnderecoWriteOnly, IEnderecoReadOnly
{
	private readonly PetDeliveryDbContext _dbContext = dbContext;

	public async Task Add(Endereco endereco) => await _dbContext.Endereco.AddAsync(endereco);

	public void Atualize(Endereco endereco) => _dbContext.Endereco.Update(endereco);

	public void Excluir(Endereco endereco) => _dbContext.Endereco.Remove(endereco);

	public async Task<Endereco?> GetById(long usuarioId, long enderecoId) =>
		await _dbContext.Endereco
			.AsNoTracking()
			.FirstOrDefaultAsync(e => e.Id == enderecoId && e.UsuarioId == usuarioId);

	public async Task<IEnumerable<Endereco>> GetByUsuarioId(long usuarioId) =>
		await _dbContext.Endereco
			.AsNoTracking()
			.Where(e => e.UsuarioId == usuarioId)
			.ToListAsync();
}
