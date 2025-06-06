using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Pedido;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Dashboard.ObterUltimosPedidos;
public class ObterUltimosPedidosUseCase : IObterUltimosPedidosUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IPedidoReadOnly _pedidoReadOnly;
	private readonly IMapper _mapper;

	public ObterUltimosPedidosUseCase(
		IUsuarioLogado usuarioLogado,
		IPedidoReadOnly pedidoReadOnly,
		IMapper mapper)
	{
		_usuarioLogado = usuarioLogado;
		_pedidoReadOnly = pedidoReadOnly;
		_mapper = mapper;
	}

	public async Task<ResponseUltimosPedidosJson> ExecuteAsync(int topN = 5)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		IList<Dominio.Entidades.Pedido> pedidosDoDominio = await _pedidoReadOnly.GetUltimosPedidosContendoProdutosDoVendedorAsync(usuarioLogado.Id, topN);

		List<ResponseUltimoPedidoJson> pedidosResponse =
			_mapper.Map<List<ResponseUltimoPedidoJson>>(pedidosDoDominio);

		return new ResponseUltimosPedidosJson
		{
			Pedidos = pedidosResponse
		};
	}
}
