using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.ObtenhaTodosProdutos;
public interface IObtenhaTodosProdutos
{
	Task<IEnumerable<ResponseProdutoJson>> ExecuteAsync();
}
