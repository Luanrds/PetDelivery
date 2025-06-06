using Dominio.Enums;

namespace Dominio.Repositorios.Pagamento;
public interface IPagamentoWriteOnly
{
	Task Adicionar(Entidades.Pagamento pagamento);
	Task AtualizarStatus(long pagamentoId, StatusPagamento status);
}
