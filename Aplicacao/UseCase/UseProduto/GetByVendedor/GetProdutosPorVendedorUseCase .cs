using Aplicacao.Extensoes;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.GetByVendedor;
public class GetProdutosPorVendedorUseCase : IGetProdutosPorVendedorUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IProdutoReadOnly _produtoRepository;
	private readonly IMapper _mapper;
	private readonly IBlobStorageService _blobStorageService;

	public GetProdutosPorVendedorUseCase(
		IUsuarioLogado usuarioLogado,
		IProdutoReadOnly produtoRepository,
		IMapper mapper,
		IBlobStorageService blobStorageService)
	{
		_usuarioLogado = usuarioLogado;
		_produtoRepository = produtoRepository;
		_mapper = mapper;
		_blobStorageService = blobStorageService;
	}

	public async Task<ResponseProdutosJson> ExecuteAsync()
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		List<Produto> produtos = await _produtoRepository.GetByUsuarioIdAsync(usuarioLogado.Id);

		if (produtos == null || produtos.Count == 0)
		{
			throw new NotFoundException("Nenhum produto encontrado para este vendedor.");
		}

		return new ResponseProdutosJson
		{
			Produtos = await produtos.MapToUserSpecificProdutoJson(usuarioLogado, _blobStorageService, _mapper)
		};
	}
}