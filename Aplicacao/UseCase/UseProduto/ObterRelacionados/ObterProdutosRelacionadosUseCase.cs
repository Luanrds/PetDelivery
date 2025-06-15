using Aplicacao.Extensoes;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.ObterRelacionados;
public class ObterProdutosRelacionadosUseCase : IObterProdutosRelacionadosUseCase
{
	private readonly IProdutoReadOnly _repository;
	private readonly IMapper _mapper;
	private readonly IBlobStorageService _blobStorageService;

	public ObterProdutosRelacionadosUseCase(IProdutoReadOnly repository, IMapper mapper, IBlobStorageService blobStorageService)
	{
		_repository = repository;
		_mapper = mapper;
		_blobStorageService = blobStorageService;
	}

	public async Task<ResponseProdutosJson> ExecuteAsync(long produtoId, int itensPorPagina)
	{
		Produto produtoPrincipal = await _repository.GetById(produtoId)
			?? throw new NotFoundException("Produto principal não encontrado.");

		IList<Produto> produtosRelacionados = await _repository.ObterRelacionadosAsync(produtoId, produtoPrincipal.Categoria, itensPorPagina);

		IList<ResponseProdutoJson> produtosMapeados = await produtosRelacionados.MapToPublicProdutoJson(_blobStorageService, _mapper);

		return new ResponseProdutosJson
		{
			Produtos = produtosMapeados,
			Total = produtosMapeados.Count,
			PaginaAtual = 1,
			TotalPaginas = 1
		};
	}
}