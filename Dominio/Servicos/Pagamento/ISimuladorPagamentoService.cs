namespace Dominio.Servicos.Pagamento;
public interface ISimuladorPagamentoService
{
	Task SimularConfirmacaoPagamentoAsync(long pedidoId);
}
