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
		if (produtos == null || !produtos.Any())
		{
			return [];
		}

		IEnumerable<Task<ResponseProdutoJson>> tasks = produtos.Select(async produto =>
		{
			ResponseProdutoJson resposta = mapper.Map<ResponseProdutoJson>(produto);

			if (produto.ImagensIdentificadores != null && produto.ImagensIdentificadores.Count != 0)
			{
				string primeiraImagemId = produto.ImagensIdentificadores.First();
				if (primeiraImagemId.NotEmpty() && produto.Usuario != null)
				{
					resposta.ImagemUrl = await blobStorageService.GetImageUrl(produto.Usuario.IdentificadorDoUsuario.ToString(), primeiraImagemId);
				}

				resposta.ImagensUrl = [];
				foreach (string imagemId in produto.ImagensIdentificadores)
				{
					if (imagemId.NotEmpty() && produto.Usuario != null)
					{
						resposta.ImagensUrl.Add(await blobStorageService.GetImageUrl(produto.Usuario.IdentificadorDoUsuario.ToString(), imagemId));
					}
				}
			}
			else
			{
				resposta.ImagemUrl = null;
				resposta.ImagensUrl = [];
			}

			return resposta;
		});

		return await Task.WhenAll(tasks);
	}

	public static async Task<IList<ResponseProdutoJson>> MapToUserSpecificProdutoJson(
		this IList<Produto> produtos,
		Usuario usuarioLogado,
		IBlobStorageService blobStorageService,
		IMapper mapper)
	{
		if (produtos == null || !produtos.Any())
		{
			return [];
		}

		IEnumerable<Task<ResponseProdutoJson>> tasks = produtos.Select(async produto =>
		{
			ResponseProdutoJson resposta = mapper.Map<ResponseProdutoJson>(produto);

			if (produto.ImagensIdentificadores != null && produto.ImagensIdentificadores.Count != 0)
			{
				string primeiraImagemId = produto.ImagensIdentificadores.First();
				if (primeiraImagemId.NotEmpty())
				{
					resposta.ImagemUrl = await blobStorageService.GetFileUrl(usuarioLogado, primeiraImagemId);
				}

				resposta.ImagensUrl = [];
				foreach (string imagemId in produto.ImagensIdentificadores)
				{
					if (imagemId.NotEmpty())
					{
						resposta.ImagensUrl.Add(await blobStorageService.GetFileUrl(usuarioLogado, imagemId));
					}
				}
			}
			else
			{
				resposta.ImagemUrl = null;
				resposta.ImagensUrl = [];
			}

			return resposta;
		});

		return await Task.WhenAll(tasks);
	}
}