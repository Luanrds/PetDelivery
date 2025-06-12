using Dominio.Entidades;
using Dominio.Repositorios.Pagamento;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Repositorio.Repositorios;

public class MetodoPagamentoUsuarioRepository(PetDeliveryDbContext dbContext) : IMetodoPagamentoUsuarioRepository
{
	private readonly PetDeliveryDbContext _dbContext = dbContext;

	public async Task Adicionar(MetodoPagamentoUsuario metodoPagamento) => 
		await _dbContext.MetodoPagamentoUsuario.AddAsync(metodoPagamento);

	public async Task<IList<MetodoPagamentoUsuario>> ObterPorUsuarioId(long usuarioId) => 
		await _dbContext.MetodoPagamentoUsuario
			.AsNoTracking()
			.Where(mp => mp.UsuarioId == usuarioId)
			.ToListAsync();

	public async Task<MetodoPagamentoUsuario?> ObterPorIdEUsuarioId(long id, long usuarioId) => 
		await _dbContext.MetodoPagamentoUsuario
			.FirstOrDefaultAsync(mp => mp.Id == id && mp.UsuarioId == usuarioId);

	public void Excluir(MetodoPagamentoUsuario metodoPagamento) =>
		_dbContext.MetodoPagamentoUsuario.Remove(metodoPagamento);
}
