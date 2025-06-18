using Dominio.Enums;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Request;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.Desconto;
public class AplicarDescontoUseCase : IAplicarDescontoUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IProdutoUpdateOnly _produtoUpdateOnly;
	private readonly IUnitOfWork _unitOfWork;

	public AplicarDescontoUseCase(IUsuarioLogado usuarioLogado, IProdutoUpdateOnly produtoUpdateOnly, IUnitOfWork unitOfWork)
	{
		_usuarioLogado = usuarioLogado;
		_produtoUpdateOnly = produtoUpdateOnly;
		_unitOfWork = unitOfWork;
	}

	public async Task Execute(long produtoId, RequestAplicarDescontoJson request)
	{
		var usuario = await _usuarioLogado.Usuario();
		var produto = await _produtoUpdateOnly.GetById(produtoId);

		if (produto is null || produto.UsuarioId != usuario.Id)
		{
			throw new UnauthorizedException("Você não tem permissão para alterar este produto.");
		}

		if (!request.ValorDesconto.HasValue || !request.TipoDesconto.HasValue || request.ValorDesconto.Value == 0)
		{
			produto.ValorDesconto = null;
			produto.TipoDesconto = null;
		}
		else
		{
			if (request.ValorDesconto.Value < 0)
				throw new ErrorOnValidationException(["O valor do desconto não pode ser negativo."]);

			var tipoDesconto = (TipoDesconto)request.TipoDesconto.Value;

			if (tipoDesconto == TipoDesconto.Porcentagem && request.ValorDesconto.Value > 100)
				throw new ErrorOnValidationException(["O desconto em porcentagem não pode ser maior que 100."]);

			if (tipoDesconto == TipoDesconto.ValorFixo && request.ValorDesconto.Value >= produto.Valor)
				throw new ErrorOnValidationException(["O valor do desconto não pode ser maior ou igual ao preço do produto."]);

			produto.TipoDesconto = tipoDesconto;
			produto.ValorDesconto = request.ValorDesconto.Value;
		}

		_produtoUpdateOnly.Atualize(produto);
		await _unitOfWork.Commit();
	}
}
