using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.ObetnhaProdutoPorCategoria;
public interface IObtenhaProdutosPorCategoria
{
	Task<IEnumerable<ResponseProdutoJson>> Execute(string categoria);
}
