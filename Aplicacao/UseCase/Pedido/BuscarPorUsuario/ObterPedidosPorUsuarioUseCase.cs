using AutoMapper;
using Dominio.Repositorios.Pedido;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Pedido.BuscarPorUsuario;
public class ObterPedidosPorUsuarioUseCase : IObterPedidosPorUsuarioUseCase
{
	private readonly IPedidoReadOnly _repository;
	private readonly IMapper _mapper;

	public ObterPedidosPorUsuarioUseCase(IPedidoReadOnly repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<ResponseListaPedidosJson> Execute(long usuarioId)
	{
		IList<Dominio.Entidades.Pedido> pedidos = await _repository.GetByUsuarioIdAsync(usuarioId);

		return new ResponseListaPedidosJson
		{
			Pedidos = _mapper.Map<List<ResponsePedidoJson>>(pedidos)
		};
	}
}
