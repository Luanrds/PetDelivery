using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.ObterRelacionados;
public interface IObterProdutosRelacionadosUseCase
{
	Task<ResponseProdutosJson> ExecuteAsync(long produtoId, int limite);
}