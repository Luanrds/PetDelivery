using Aplicacao.Extensoes;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.ObtenhaTodosProdutos;
public class ObtenhaTodosProdutos : IObtenhaTodosProdutos
{
	private readonly IMapper _mapper;
	private readonly IProdutoReadOnly _repository;
	private readonly IBlobStorageService _blobStorageService;

	public ObtenhaTodosProdutos(
		IMapper mapper,
		IProdutoReadOnly repository,
		IBlobStorageService blobStorageService)
	{
		_mapper = mapper;
		_repository = repository;
		_blobStorageService = blobStorageService;
	}

	public async Task<ResponseProdutosJson> ExecuteAsync()
	{
		List<Produto> produtos = await _repository.GetAll();

		if (produtos == null || produtos.Count == 0)
		{
			return new ResponseProdutosJson
			{
				Produtos = []
			};
		}

		return new ResponseProdutosJson
		{
			Produtos = await produtos.MapToPublicProdutoJson(_blobStorageService, _mapper)
		};
	}
}
