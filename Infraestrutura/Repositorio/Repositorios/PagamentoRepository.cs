using Dominio.Entidades;
using Dominio.Enums;
using Dominio.Repositorios.Pagamento;
using Infraestrutura.Configuracao;

namespace Infraestrutura.Repositorio.Repositorios;
public class PagamentoRepository(PetDeliveryDbContext dbContext) : IPagamentoWriteOnly
{
	public async Task Adicionar(Pagamento pagamento) =>
		await dbContext.Pagamento.AddAsync(pagamento);

	public async Task AtualizarStatus(long pagamentoId, StatusPagamento status)
	{
		Pagamento? pagamento = await dbContext.Pagamento.FindAsync(pagamentoId);
		if (pagamento != null)
		{
			pagamento.StatusPagamento = status;
			dbContext.Pagamento.Update(pagamento);

		}
	}
}
