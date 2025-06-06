
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseEndereco.Buscar;
public interface IBuscarEnderecosUseCase
{
	Task<IEnumerable<ResponseEnderecoJson>> ExecuteAsync();
}
