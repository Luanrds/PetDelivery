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

	public async Task<ResponseProdutoJson> ExecuteAsync(long produtoId)
	{
		Produto produto = await _repository.GetById(produtoId)
			?? throw new NotFoundException("Produto não encontrado.");

		ResponseProdutoJson response = _mapper.Map<ResponseProdutoJson>(produto);

		if (produto.ImagensIdentificadores != null && produto.ImagensIdentificadores.Count != 0)
		{
			string primeiraImagemId = produto.ImagensIdentificadores.First();
			if (primeiraImagemId.NotEmpty())
			{
				response.ImagemUrl = await _blobStorageService.GetFileUrl(produto.Usuario, primeiraImagemId);
			}

			response.ImagensUrl = [];
			foreach (string imagemId in produto.ImagensIdentificadores)
			{
				if (imagemId.NotEmpty())
				{
					response.ImagensUrl.Add(await _blobStorageService.GetFileUrl(produto.Usuario, imagemId));
				}
			}
		}

		return response;
	}
}
