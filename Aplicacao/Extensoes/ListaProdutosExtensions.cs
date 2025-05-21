using AutoMapper;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Servicos.Storage;
using PetDelivery.Communication.Response;

namespace Aplicacao.Extensoes;
public static class ListaProdutosExtensions
{
	public static async Task<IList<ResponseProdutoJson>> MapToProdutoJson(
		this IList<Produto> produtos, 
		Usuario usuario, 
		IBlobStorageService blobStorageService,
		IMapper mapper)
	{
		IEnumerable<Task<ResponseProdutoJson>> resultado = produtos.Select(async produto =>
		{
			ResponseProdutoJson resposta = mapper.Map<ResponseProdutoJson>(produto);

			if (produto.ImagemIdentificador.NotEmpty())
			{
				resposta.ImagemUrl = await blobStorageService.GetFileUrl(usuario, produto.ImagemIdentificador);
			}

			return resposta;
		});

		ResponseProdutoJson[] resposta = await Task.WhenAll(resultado);

		return resposta;
	}
}
