using AutoMapper;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.GetById;
public class GetProdutoById : IGetProdutoById
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IMapper _mapper;
	private readonly IProdutoReadOnly _repository;
	private readonly IBlobStorageService _blobStorageService;

	public GetProdutoById(
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

	public async Task<ResponseProdutoJson> ExecuteAsync(long ProdutoId)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		var produto = await _repository.GetById(ProdutoId) //implementar para que ele passe como parametro o id do usuario logado
			?? throw new NotFoundException("Produto não encontrado.");

		ResponseProdutoJson response = _mapper.Map<ResponseProdutoJson>(produto);

		if(produto.ImagemIdentificador.NotEmpty())
		{
			var url = await _blobStorageService.GetFileUrl(usuarioLogado, produto.ImagemIdentificador);

			response.ImagemUrl = url;
		}

		return response;
	}
}
