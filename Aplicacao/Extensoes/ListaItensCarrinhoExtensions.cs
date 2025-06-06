using AutoMapper;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Servicos.Storage;
using PetDelivery.Communication.Response;

namespace Aplicacao.Extensoes;

public static class ListaItensCarrinhoExtensions
{
	public static async Task<List<ResponseItemCarrinhoJson>> MapToResponseItensCarrinhoJson(
		this IList<ItemCarrinhoDeCompra> itens,
		Usuario usuario,
		IBlobStorageService blobStorageService,
		IMapper mapper)
	{
		if (itens == null || !itens.Any())
		{
			return [];
		}

		var responseItens = new List<ResponseItemCarrinhoJson>();

		foreach (var item in itens)
		{
			ResponseItemCarrinhoJson responseItem = mapper.Map<ResponseItemCarrinhoJson>(item);

			if (item.Produto != null && item.Produto.ImagensIdentificadores != null && item.Produto.ImagensIdentificadores.Any())
			{
				var primeiraImagemId = item.Produto.ImagensIdentificadores.First();
				if (primeiraImagemId.NotEmpty())
				{
					responseItem.ImagemUrl = await blobStorageService.GetFileUrl(item.Produto.Usuario, primeiraImagemId);
				}
			}
			else
			{
				responseItem.ImagemUrl = null; 
			}
			responseItens.Add(responseItem);
		}
		return responseItens;
	}
}