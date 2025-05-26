using Aplicacao.UseCase.Carrinho.LimparCarrinho;
using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Carrinho;
using Dominio.Servicos.UsuarioLogado;

namespace Aplicacao.UseCase.Carrinho.Limpar;

public class LimpeCarrinhoUseCase : ILimpeCarrinhoUseCase
{
	private readonly ICarrinhoWriteOnly _carrinhoWriteOnly;
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IUsuarioLogado _usuarioLogado;

	public LimpeCarrinhoUseCase(
		ICarrinhoWriteOnly carrinhoWriteOnly,
		ICarrinhoReadOnly carrinhoReadOnly,
		IUnitOfWork unitOfWork,
		IUsuarioLogado usuarioLogado)
	{
		_carrinhoWriteOnly = carrinhoWriteOnly;
		_carrinhoReadOnly = carrinhoReadOnly;
		_unitOfWork = unitOfWork;
		_usuarioLogado = usuarioLogado;
	}

	public async Task ExecuteAsync()
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		CarrinhoDeCompras? carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(usuarioLogado.Id);

		if (carrinho != null && carrinho.ItensCarrinho.Count != 0)
		{
			await _carrinhoWriteOnly.LimparItensAsync(carrinho.Id);

			await _unitOfWork.Commit();
		}
	}
}
