using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.ObtenhaTodosProdutos;
public interface IObtenhaTodosProdutos
{
	Task<ResponseProdutosJson> ExecuteAsync();
}
