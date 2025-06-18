using AutoMapper;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Servicos.Storage;
using PetDelivery.Communication.Response;

namespace Aplicacao.Extensoes;

public static class ListaPedidosExtensions
{
	public static async Task<List<ResponsePedidoJson>> MapToResponsePedidoJson(
		this IList<Pedido> pedidos,
		IBlobStorageService blobStorageService,
		IMapper mapper)
	{
		if (pedidos == null || !pedidos.Any())
		{
			return [];
		}

		List<ResponsePedidoJson> responsePedidos = mapper.Map<List<ResponsePedidoJson>>(pedidos);

		var originalPedidosDict = pedidos.ToDictionary(p => p.Id);

		var urlUpdateTasks = responsePedidos.SelectMany(pedidoResponse =>
		{
			originalPedidosDict.TryGetValue(pedidoResponse.Id, out var pedidoOriginal);

			return pedidoResponse.Itens.Select(async itemResponse =>
			{
				var itemOriginal = pedidoOriginal?.Itens.FirstOrDefault(i => i.Id == itemResponse.Id);
				var produtoOriginal = itemOriginal?.Produto;

				if (produtoOriginal?.ImagensIdentificadores?.Any() == true && produtoOriginal.Usuario != null)
				{
					string imagemIdentificador = produtoOriginal.ImagensIdentificadores.First();
					if (imagemIdentificador.NotEmpty())
					{
						itemResponse.ImagemUrl = await blobStorageService.GetFileUrl(produtoOriginal.Usuario, imagemIdentificador);
					}
				}
			});
		});

		await Task.WhenAll(urlUpdateTasks);

		return responsePedidos;
	}
}