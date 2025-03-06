using Aplicacao.UseCase.Carrinho.LimparCarrinho;
using Dominio.Repositorios;
using Dominio.Repositorios.Carrinho;

namespace Aplicacao.UseCase.Carrinho.Limpar;

public class LimpeCarrinhoUseCase : ILimpeCarrinhoUseCase
{
	private readonly ICarrinhoWriteOnly _carrinhoWriteOnly;
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly IUnitOfWork _unitOfWork;

	public LimpeCarrinhoUseCase(ICarrinhoWriteOnly carrinhoWriteOnly, ICarrinhoReadOnly carrinhoReadOnly, IUnitOfWork unitOfWork)
	{
		_carrinhoWriteOnly = carrinhoWriteOnly;
		_carrinhoReadOnly = carrinhoReadOnly;
		_unitOfWork = unitOfWork;
	}

	public async Task ExecuteLimpar(long usuarioId)
	{
		var carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(usuarioId);

		if (carrinho != null)
		{
			await _carrinhoWriteOnly.LimparCarrinho(carrinho);
			await _unitOfWork.Commit();
		}
	}
}
