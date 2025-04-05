using Dominio.Entidades;

namespace Aplicacao.UseCase.Carrinho.LimparCarrinho;
public interface ILimpeCarrinhoUseCase
{
	Task ExecuteLimpar(long usuarioId);
}
