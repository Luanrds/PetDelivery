using Dominio.Entidades;
using Dominio.Repositorios.Endereco;
using Infraestrutura.Configuracao;

namespace Infraestrutura.Repositorio.Repositorios;

public class EnderecoRepository(PetDeliveryDbContext dbContext) : IEnderecoWriteOnly
{
	private readonly PetDeliveryDbContext _dbContext = dbContext;

	public async Task Add(Endereco endereco) => await _dbContext.Endereco.AddAsync(endereco);
}
