using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.GetByVendedor;
public interface IGetProdutosPorVendedorUseCase
{
	Task<IEnumerable<ResponseProdutoJson>> ExecuteAsync();
}
