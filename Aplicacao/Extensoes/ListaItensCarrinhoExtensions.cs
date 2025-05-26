using AutoMapper;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Servicos.Storage;
using PetDelivery.Communication.Response;

namespace Aplicacao.Extensoes;

public static class ListaItensCarrinhoExtensions
{
	public static async Task<List<ResponseItemCarrinhoJson>> MapToResponseItemCarrinhoJsonComImagens(
		this IList<ItemCarrinhoDeCompra> itensCarrinho,
		IBlobStorageService blobStorageService,
		IMapper mapper)
	{
		if (itensCarrinho == null || !itensCarrinho.Any())
		{
			return [];
		}

		var tasks = itensCarrinho.Select(async itemOriginal =>
		{
			var itemResponse = mapper.Map<ResponseItemCarrinhoJson>(itemOriginal);

			if (itemOriginal.Produto != null &&
				itemOriginal.Produto.Usuario != null &&
				itemOriginal.Produto.ImagemIdentificador.NotEmpty())
			{
				string nomeContainerDoVendedor = itemOriginal.Produto.Usuario.IdentificadorDoUsuario.ToString();

				itemResponse.ImagemUrl = await blobStorageService.GetImageUrl(nomeContainerDoVendedor, itemOriginal.Produto.ImagemIdentificador);
			}
			return itemResponse;
		});

		var resultados = await Task.WhenAll(tasks);
		return [.. resultados];
	}
}