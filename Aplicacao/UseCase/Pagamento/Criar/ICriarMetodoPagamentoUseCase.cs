using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Pagamento.Criar;
public interface ICriarMetodoPagamentoUseCase
{
	Task<ResponseCartaoCreditoJson> Execute(RequestCartaoCreditoJson request);
}
