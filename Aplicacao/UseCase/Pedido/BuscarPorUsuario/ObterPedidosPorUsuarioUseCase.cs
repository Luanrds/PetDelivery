using Aplicacao.Extensoes;
using AutoMapper;
using Dominio.Repositorios.Pedido;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Pedido.BuscarPorUsuario;
public class ObterPedidosPorUsuarioUseCase : IObterPedidosPorUsuarioUseCase
{
	private readonly IPedidoReadOnly _pedidoRepository;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IMapper _mapper;
	private readonly IBlobStorageService _storageService;

	public ObterPedidosPorUsuarioUseCase(
		IPedidoReadOnly pedidoRepository,
		IUsuarioLogado usuarioLogado,
		IMapper mapper,
		IBlobStorageService storageService)
	{
		_pedidoRepository = pedidoRepository;
		_usuarioLogado = usuarioLogado;
		_mapper = mapper;
		_storageService = storageService;
	}

	public async Task<ResponseListaPedidosJson> Execute()
	{
		Dominio.Entidades.Usuario usuarioLogado = await _usuarioLogado.Usuario();

		IList<Dominio.Entidades.Pedido> pedidos = await _pedidoRepository.GetByUsuarioIdAsync(usuarioLogado.Id);

		var responsePedidos = await pedidos.MapToResponsePedidoJson(_storageService, _mapper);

		return new ResponseListaPedidosJson { Pedidos = responsePedidos };
	}
}
