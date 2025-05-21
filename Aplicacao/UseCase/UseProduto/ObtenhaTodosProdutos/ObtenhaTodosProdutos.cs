using Aplicacao.Extensoes;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.ObtenhaTodosProdutos;
public class ObtenhaTodosProdutos : IObtenhaTodosProdutos
{
	private readonly IMapper _mapper;
	private readonly IProdutoReadOnly _repository;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IBlobStorageService _blobStorageService;

	public ObtenhaTodosProdutos(
		IMapper mapper, 
		IProdutoReadOnly repository,
		IUsuarioLogado usuarioLogado,
		IBlobStorageService blobStorageService)
	{
		_mapper = mapper;
		_repository = repository;
		_usuarioLogado = usuarioLogado;
		_blobStorageService = blobStorageService;
	}

	async Task<ResponseProdutosJson> IObtenhaTodosProdutos.ExecuteAsync()
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		List<Produto> produtos = await _repository.GetAll();

		if (produtos.Count == 0)
		{
			throw new NotFoundException("Nenhum produto encontrado.");
		}

		return new ResponseProdutosJson
		{
			Produtos = await produtos.MapToProdutoJson(usuarioLogado, _blobStorageService, _mapper)
		};
	}
}
