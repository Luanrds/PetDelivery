using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.Facades;
public interface IProdutoFacade
{
	public Task<ResponseProdutoJson> CrieProduto(RequestProdutoJson request);
	public Task<ResponseProdutoJson> ObtenhaProdutoPeloId(long ProdutoId);
	public Task<IEnumerable<ResponseProdutoJson>> ObtenhaProduto();
	public Task ExcluirProduto(long produtoId);
	public Task Atualize(long produtoId, RequestProdutoJson requisicao);
}
