using Aplicacao.UseCase.Pedido.BuscarPorUsuario;
using Aplicacao.UseCase.Pedido.Criar;
using Aplicacao.UseCase.Pedido.ObterPedido;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.API.Atributos;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;

[UsuarioAutenticado]
public class PedidoController : PetDeliveryBaseController
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponsePedidoCriadoJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> CriarPedido(
		[FromServices] ICriarPedidoUseCase useCase,
		[FromBody] RequestCheckoutJson request)
	{
		ResponsePedidoCriadoJson resposta = await useCase.Execute(request);

		return CreatedAtAction(nameof(GetPedidoPorId), new { pedidoId = resposta.PedidoId }, resposta);
	}

	[HttpGet("{pedidoId:long}", Name = "GetPedidoPorId")]
	[ProducesResponseType(typeof(ResponsePedidoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetPedidoPorId(
		[FromServices] IObterPedidoPorIdUseCase useCase,
		[FromRoute] long pedidoId)
	{
		ResponsePedidoJson resposta = await useCase.Execute(pedidoId);
		return Ok(resposta);
	}

	[HttpGet]
	[ProducesResponseType(typeof(ResponseListaPedidosJson), StatusCodes.Status200OK)]
	public async Task<IActionResult> GetPedidosPorUsuario(
		[FromServices] IObterPedidosPorUsuarioUseCase useCase)
	{
		ResponseListaPedidosJson resposta = await useCase.Execute();
		return Ok(resposta);
	}
}
