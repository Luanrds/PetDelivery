using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.GetByVendedor;
public interface IGetProdutosPorVendedorUseCase
{
	Task<ResponseProdutosJson> ExecuteAsync();
}
