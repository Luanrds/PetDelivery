using Aplicacao.Extensoes;
using AutoMapper;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.ObetnhaProdutoPorCategoria;

public class ObtenhaProdutosPorCategoriaUseCase : IObtenhaProdutosPorCategoria
{
	private readonly IProdutoReadOnly _produtoReadOnly;
	private readonly IMapper _mapper;
	private readonly IBlobStorageService _blobStorageService;

	public ObtenhaProdutosPorCategoriaUseCase(
		IProdutoReadOnly produtoReadOnlyRepositorio,
		IMapper mapper,
		IBlobStorageService blobStorageService)
	{
		_produtoReadOnly = produtoReadOnlyRepositorio;
		_mapper = mapper;
		_blobStorageService = blobStorageService;
	}

	public async Task<IEnumerable<ResponseProdutoJson>> ExecuteAsync(string categoria)
	{
		var produtos = (await _produtoReadOnly.ObterPorCategoria(categoria)).ToList();

		if (produtos == null || produtos.Count == 0)
		{
			return [];
		}

		return await produtos.MapToPublicProdutoJson(_blobStorageService, _mapper);
	}
}