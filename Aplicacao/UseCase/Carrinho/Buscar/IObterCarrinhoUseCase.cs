using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Carrinho.Buscar;
public interface IObterCarrinhoUseCase
{
	Task<ResponseCarrinhoDeComprasJson> ExecuteAsync();
}
