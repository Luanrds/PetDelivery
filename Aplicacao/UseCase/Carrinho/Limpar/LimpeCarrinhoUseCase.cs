using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios;

namespace Aplicacao.UseCase.Carrinho.LimparCarrinho;
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

	public async Task ExecuteLimpar()
	{
		var carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo();

		if(carrinho is not null)
			await _carrinhoWriteOnly.LimparCarrinho(carrinho);
			await _unitOfWork.Commit();
	}
}
