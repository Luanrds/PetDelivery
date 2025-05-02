using AutoMapper;
using Dominio.Repositorios.Pedido;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.Pedido.ObterPedido;
public class ObterPedidoPorIdUseCase : IObterPedidoPorIdUseCase
{
	private readonly IPedidoReadOnly _repository;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado;

	public ObterPedidoPorIdUseCase(
		IPedidoReadOnly repository,
		IMapper mapper,
		IUsuarioLogado usuarioLogado)
	{
		_repository = repository;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
	}

	public async Task<ResponsePedidoJson> Execute(long pedidoId)
	{
		Dominio.Entidades.Usuario usuarioLogado = await _usuarioLogado.Usuario();

		Dominio.Entidades.Pedido? pedido = await _repository.GetByIdAsync(pedidoId) 
			?? throw new NotFoundException($"Pedido com ID {pedidoId} não encontrado.");

		// TODO: Adicionar lógica se Vendedores/Admins podem ver pedidos de outros
		if (pedido.UsuarioId != usuarioLogado.Id)
		{
			throw new NotFoundException($"Pedido com ID {pedidoId} não encontrado.");
		}

		return _mapper.Map<ResponsePedidoJson>(pedido);
	}
}
