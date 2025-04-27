
namespace Aplicacao.UseCase.UseEndereco.Excluir;
public interface IExcluirEnderecoUseCase
{
	Task ExecuteAsync(long usuatioId, long enderecoId);
}
