using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.ObterEmPromocao;
public interface IObterProdutosEmPromocaoUseCase
{
	Task<ResponseProdutosJson> ExecuteAsync(int pagina, int itensPorPagina);
}
