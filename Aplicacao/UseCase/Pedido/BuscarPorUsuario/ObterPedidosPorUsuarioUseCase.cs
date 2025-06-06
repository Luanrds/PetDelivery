using AutoMapper;
using Dominio.Repositorios.Pedido;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Pedido.BuscarPorUsuario;
public class ObterPedidosPorUsuarioUseCase : IObterPedidosPorUsuarioUseCase
{
	private readonly IPedidoReadOnly _repository;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado;

	public ObterPedidosPorUsuarioUseCase(
		IPedidoReadOnly repository,
		IMapper mapper,
		IUsuarioLogado usuarioLogado)
	{
		_repository = repository;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
	}

	public async Task<ResponseListaPedidosJson> Execute()
	{
		Dominio.Entidades.Usuario usuarioLogado = await _usuarioLogado.Usuario();

		IList<Dominio.Entidades.Pedido> pedidos = await _repository.GetByUsuarioIdAsync(usuarioLogado.Id);

		return new ResponseListaPedidosJson
		{
			Pedidos = _mapper.Map<List<ResponsePedidoJson>>(pedidos)
		};
	}
}
