using Dominio.Entidades;
using Dominio.Repositorios.Endereco;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;

public class EnderecoRepository(PetDeliveryDbContext dbContext) : IEnderecoWriteOnly, IEnderecoReadOnly
{
	private readonly PetDeliveryDbContext _dbContext = dbContext;

	public async Task Add(Endereco endereco) => await _dbContext.Endereco.AddAsync(endereco);

	public async Task<IEnumerable<Endereco>> GetByUsuarioId(long usuarioId) =>
		await _dbContext.Endereco.Where(e => e.UsuarioId == usuarioId).ToListAsync();
}
