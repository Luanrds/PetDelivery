using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.GetById;
public interface IGetProdutoById
{
	Task<ResponseProdutoJson> Execute(long ProdutoId);
}
