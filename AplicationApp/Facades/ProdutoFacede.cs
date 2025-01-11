using AutoMapper;
using Dominio.Repositorios.Produto;
using Dominio.Repositorios;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using Aplicacao.Validadores;
using PetDelivery.Exceptions.ExceptionsBase;
using Dominio.Entidades;

namespace Aplicacao.Facades;
public class ProdutoFacede : IProdutoFacade
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IProdutoUpdateOnly _reposioryUpdate;
	private readonly IProdutoWriteOnly _repositoryWriteOnly;
	private readonly IProdutoReadOnly _repositoryReadOnly;

	public ProdutoFacede(
		IUnitOfWork unitOfWork,
		IMapper mapper,
		IProdutoUpdateOnly reposioryUpdate,
		IProdutoWriteOnly repositoryWriteOnly,
		IProdutoReadOnly repositoryReadOnly)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_reposioryUpdate = reposioryUpdate;
		_repositoryWriteOnly = repositoryWriteOnly;
		_repositoryReadOnly = repositoryReadOnly;
	}

	public async Task Atualize(long produtoId, RequestProdutoJson requisicao)
	{
		Validate(requisicao);

		var produto = await _reposioryUpdate.GetById(produtoId);

		if (produto is null)
		{
			throw new NotFoundException("Produto não encontrado.");
		}

		_mapper.Map(requisicao, produto);

		_reposioryUpdate.Atualize(produto);

		await _unitOfWork.Commit();
	}

	public async Task<ResponseProdutoJson> CrieProduto(RequestProdutoJson request)
	{
		Validate(request);

		var produto = _mapper.Map<Produto>(request);

		await _repositoryWriteOnly.Add(produto);

		await _unitOfWork.Commit();

		return _mapper.Map<ResponseProdutoJson>(produto);
	}

	public async Task ExcluirProduto(long produtoId)
	{
		var produto = await _repositoryReadOnly.GetById(produtoId);

		if (produto is null)
		{
			throw new NotFoundException("Produto não encontrado.");
		}

		await _repositoryWriteOnly.Excluir(produtoId);

		await _unitOfWork.Commit();
	}

	public async Task<IEnumerable<ResponseProdutoJson>> ObtenhaProduto()
	{
		var produtos = await _repositoryReadOnly.GetAll();

		if (produtos.Count == 0)
		{
			throw new NotFoundException("Nenhum produto encontrado.");
		}

		return _mapper.Map<IEnumerable<ResponseProdutoJson>>(produtos);
	}

	public async Task<ResponseProdutoJson> ObtenhaProdutoPeloId(long ProdutoId)
	{
		var produto = await _repositoryReadOnly.GetById(ProdutoId);

		if (produto is null)
		{
			throw new NotFoundException("Produto não encontrado.");
		}

		var response = _mapper.Map<ResponseProdutoJson>(produto);

		return response;
	}

	private static void Validate(RequestProdutoJson request)
	{
		var validator = new ProdutoValidator();

		var result = validator.Validate(request);

		if (result.IsValid == false)
		{
			var mensagensDeErro = result.Errors.Select(e => e.ErrorMessage).ToList();

			throw new ErrorOnValidationException(mensagensDeErro);
		}
	}
}
