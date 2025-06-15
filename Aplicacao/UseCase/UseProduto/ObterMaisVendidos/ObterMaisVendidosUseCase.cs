using Aplicacao.Extensoes;
using AutoMapper;
using Dominio.Repositorios.Pedido;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.ObterMaisVendidos;
public class ObterMaisVendidosUseCase : IObterMaisVendidosUseCase
{
	private readonly IPedidoReadOnly _pedidoRepository;
	private readonly IProdutoReadOnly _produtoRepository;
	private readonly IMapper _mapper;
	private readonly IBlobStorageService _blobStorageService;

	public ObterMaisVendidosUseCase(
		IPedidoReadOnly pedidoRepository,
		IProdutoReadOnly produtoRepository,
		IMapper mapper,
		IBlobStorageService blobStorageService)
	{
		_pedidoRepository = pedidoRepository;
		_produtoRepository = produtoRepository;
		_mapper = mapper;
		_blobStorageService = blobStorageService;
	}

	public async Task<ResponseProdutosJson> ExecuteAsync(int limite)
	{
		var maisVendidosInfo = await _pedidoRepository.ObterProdutosMaisVendidos(limite);

		if (maisVendidosInfo is null || !maisVendidosInfo.Any())
		{
			return new ResponseProdutosJson { Produtos = [] };
		}

		var produtoIds = maisVendidosInfo.Select(p => p.ProdutoId);

		var produtos = await _produtoRepository.ObterPorIdsAsync(produtoIds);

		var produtosDicionario = produtos.ToDictionary(p => p.Id);
		var produtosOrdenados = maisVendidosInfo
			.Select(info => produtosDicionario.GetValueOrDefault(info.ProdutoId))
			.Where(p => p is not null)
			.ToList();

		IList<ResponseProdutoJson> produtosMapeados = await produtosOrdenados.MapToPublicProdutoJson(_blobStorageService, _mapper);

		return new ResponseProdutosJson
		{
			Produtos = produtosMapeados,
			Total = produtosMapeados.Count,
			PaginaAtual = 1,
			TotalPaginas = 1
		};
	}
}