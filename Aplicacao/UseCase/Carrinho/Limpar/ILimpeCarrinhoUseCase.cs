using Dominio.Entidades;

namespace Aplicacao.UseCase.Carrinho.LimparCarrinho;
public interface ILimpeCarrinhoUseCase
{
	Task ExecuteAsync(long usuarioId);
}
