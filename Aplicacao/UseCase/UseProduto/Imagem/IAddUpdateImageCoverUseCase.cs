using Microsoft.AspNetCore.Http;

namespace Aplicacao.UseCase.UseProduto.Imagem;
public interface IAddUpdateImageCoverUseCase
{
	Task Execute(long produtoId, IFormFile file);
}
