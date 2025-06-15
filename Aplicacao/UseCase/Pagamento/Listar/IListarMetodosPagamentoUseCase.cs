using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Pagamento.Listar;
public interface IListarMetodosPagamentoUseCase
{
	Task<IList<ResponseCartaoCreditoJson>> Execute();
}
