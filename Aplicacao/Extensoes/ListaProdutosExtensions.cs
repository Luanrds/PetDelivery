using AutoMapper;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Servicos.Storage;
using PetDelivery.Communication.Response;

namespace Aplicacao.Extensoes;
public static class ListaProdutosExtensions
{
	public static async Task<IList<ResponseProdutoJson>> MapToPublicProdutoJson(
		this IList<Produto> produtos,
		IBlobStorageService blobStorageService,
		IMapper mapper)
	{
		IEnumerable<Task<ResponseProdutoJson>> resultado = produtos.Select(async produto =>
		{
			ResponseProdutoJson resposta = mapper.Map<ResponseProdutoJson>(produto);

			if (produto.ImagemIdentificador.NotEmpty() && produto.Usuario != null)
			{
				resposta.ImagemUrl = await blobStorageService.GetImageUrl(produto.Usuario.IdentificadorDoUsuario.ToString(), produto.ImagemIdentificador);
			}

			return resposta;
		});

		ResponseProdutoJson[] resposta = await Task.WhenAll(resultado);

		return resposta;
	}

	public static async Task<IList<ResponseProdutoJson>> MapToUserSpecificProdutoJson(
		this IList<Produto> produtos,
		Usuario usuarioLogado,
		IBlobStorageService blobStorageService,
		IMapper mapper)
	{
		IEnumerable<Task<ResponseProdutoJson>> resultado = produtos.Select(async produto =>
		{
			ResponseProdutoJson resposta = mapper.Map<ResponseProdutoJson>(produto);

			if (produto.ImagemIdentificador.NotEmpty())
			{
				resposta.ImagemUrl = await blobStorageService.GetFileUrl(usuarioLogado, produto.ImagemIdentificador);
			}

			return resposta;
		});

		ResponseProdutoJson[] resposta = await Task.WhenAll(resultado);

		return resposta;
	}
}
