using PetDelivery.Communication.Request;

namespace Aplicacao.UseCase.UseProduto.Desconto;
public interface IAplicarDescontoUseCase
{
	Task Execute(long produtoId, RequestAplicarDescontoJson request);
}
