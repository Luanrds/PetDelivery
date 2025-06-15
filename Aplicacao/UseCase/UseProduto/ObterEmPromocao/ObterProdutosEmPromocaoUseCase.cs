using Aplicacao.Extensoes;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.ObterEmPromocao;
public class ObterProdutosEmPromocaoUseCase : IObterProdutosEmPromocaoUseCase
{
	private readonly IProdutoReadOnly _repository;
	private readonly IMapper _mapper;
	private readonly IBlobStorageService _blobStorageService;

	public ObterProdutosEmPromocaoUseCase(IProdutoReadOnly repository, IMapper mapper, IBlobStorageService blobStorageService)
	{
		_repository = repository;
		_mapper = mapper;
		_blobStorageService = blobStorageService;
	}

	public async Task<ResponseProdutosJson> ExecuteAsync(int pagina, int itensPorPagina)
	{
		(IList<Produto> produtos, int totalItens) = await _repository.ObterEmPromocaoAsync(pagina, itensPorPagina);

		return new ResponseProdutosJson
		{
			Produtos = await produtos.MapToPublicProdutoJson(_blobStorageService, _mapper),
			Total = totalItens,
			PaginaAtual = pagina,
			TotalPaginas = (int)Math.Ceiling(totalItens / (double)itensPorPagina)
		};
	}
}