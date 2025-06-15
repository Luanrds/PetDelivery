using AutoMapper;
using Dominio.ObjetosDeValor;
using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using Aplicacao.Extensoes;
using Dominio.Servicos.Storage;

namespace Aplicacao.UseCase.UseProduto.Buscar;
public class BuscarProdutosUseCase : IBuscarProdutosUseCase
{
	private readonly IProdutoReadOnly _repository;
	private readonly IMapper _mapper;
	private readonly IBlobStorageService _blobStorageService;
	public BuscarProdutosUseCase(IProdutoReadOnly repository, IMapper mapper, IBlobStorageService blobStorageService)
	{
		_repository = repository;
		_mapper = mapper;
		_blobStorageService = blobStorageService;
	}

	public async Task<ResponseProdutosJson> Execute(RequestBuscaProdutosJson request)
	{
		BuscaProdutosCriteria criteria = new()
		{
			Termo = request.Termo,
			PrecoMin = request.PrecoMin,
			PrecoMax = request.PrecoMax,
			OrdenarPor = request.OrdenarPor,
			Pagina = request.Pagina,
			ItensPorPagina = request.ItensPorPagina
		};

		(IList<Produto> produtos, int totalItens) = await _repository.Buscar(criteria);

		return new ResponseProdutosJson
		{
			Produtos = await produtos.MapToPublicProdutoJson(_blobStorageService, _mapper),
			Total = totalItens,
			PaginaAtual = request.Pagina,
			TotalPaginas = (int)Math.Ceiling(totalItens / (double)request.ItensPorPagina)
		};
	}
}
