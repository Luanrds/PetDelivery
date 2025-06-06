using Aplicacao.Validadores;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Request;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.Atualizar;
public class AtualizeProdutoUseCase : IAtualizeProdutoUseCase
{
	private readonly IProdutoUpdateOnly _repositorioUpdate;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado;

	public AtualizeProdutoUseCase(
			IProdutoUpdateOnly repositorioUpdate,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			IUsuarioLogado usuarioLogado)
	{
		_repositorioUpdate = repositorioUpdate;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
	}

	public async Task ExecuteAsync(long produtoId, RequestProdutoJson requisicao)
	{
		Usuario vendedorLogado = await _usuarioLogado.Usuario();

		Validate(requisicao);

		Produto produto = await _repositorioUpdate.GetById(produtoId)
			?? throw new NotFoundException($"Produto com ID {produtoId} não encontrado.");

		if (produto.UsuarioId != vendedorLogado.Id)
		{
			throw new UnauthorizedException("Você não tem permissão para alterar este produto.");
		}

		_mapper.Map(requisicao, produto);

		_repositorioUpdate.Atualize(produto);

		await _unitOfWork.Commit();
	}

	public static void Validate(RequestProdutoJson requisicao)
	{
		ProdutoValidator validator = new();

		FluentValidation.Results.ValidationResult resultado = validator.Validate(requisicao);

		if (resultado.IsValid == false)
		{
			List<string> mensagensDeErro = resultado.Errors.Select(e => e.ErrorMessage).ToList();

			throw new ErrorOnValidationException(mensagensDeErro);
		}
	}
}
