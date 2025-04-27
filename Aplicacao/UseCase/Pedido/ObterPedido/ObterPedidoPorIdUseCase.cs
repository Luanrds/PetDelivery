using AutoMapper;
using Dominio.Repositorios.Pedido;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.Pedido.ObterPedido;
public class ObterPedidoPorIdUseCase : IObterPedidoPorIdUseCase
{
	private readonly IPedidoReadOnly _repository;
	private readonly IMapper _mapper;

	public ObterPedidoPorIdUseCase(IPedidoReadOnly repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<ResponsePedidoJson> Execute(long pedidoId)
	{
		Dominio.Entidades.Pedido? pedido = await _repository.GetByIdAsync(pedidoId)
			?? throw new NotFoundException($"Pedido com ID {pedidoId} não encontrado.");

		return _mapper.Map<ResponsePedidoJson>(pedido);
	}
}
