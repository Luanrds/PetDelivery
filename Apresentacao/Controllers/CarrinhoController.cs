using Aplicacao.UseCase.Carrinho.Atualizar;
using Aplicacao.UseCase.Carrinho.Buscar;
using Aplicacao.UseCase.Carrinho.Criar;
using Aplicacao.UseCase.Carrinho.LimparCarrinho;
using Aplicacao.UseCase.Carrinho.RemoverItem;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;
public class CarrinhoController : PetDeliveryBaseController
{
	[HttpPost]
	[Route("item")]
	[ProducesResponseType(typeof(ResponseCarrinhoDeComprasJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> AdicioneItemCarrinho(
		[FromServices] ICarrinhoUseCase useCase,
		[FromBody] RequestItemCarrinhoJson request)
	{
		ResponseCarrinhoDeComprasJson resposta = await useCase.ExecuteAsync(request);

		return Created(string.Empty, resposta);
	}

	[HttpGet]
	[ProducesResponseType(typeof(ResponseCarrinhoDeComprasJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ObterCarrinho(
		[FromServices] IObterCarrinhoUseCase useCase,
		[FromQuery] long usuarioId)
	{
		ResponseCarrinhoDeComprasJson resposta = await useCase.ExecuteAsync(usuarioId);

		return Ok(resposta);
	}

	[HttpPut]
	[Route("item/{itemId}")]
	[ProducesResponseType(typeof(ResponseCarrinhoDeComprasJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> AtualizarQuantidadeItemCarrinho(
		[FromServices] IAtualizeQtdItemCarrinhoUseCase useCase,
		[FromRoute] long itemId,
		[FromBody] RequestAtualizarItemCarrinhoJson request)
	{
		ResponseCarrinhoDeComprasJson resposta = await useCase.ExecuteAsync(itemId, request);

		return Ok(resposta);
	}

	[HttpDelete]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> LimparCarrinho(
		[FromServices] ILimpeCarrinhoUseCase useCase,
		[FromQuery] long usuarioId)
	{
		await useCase.ExecuteAsync(usuarioId);

		return NoContent();
	}

	[HttpDelete]
	[Route("item/{itemId}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> RemoverItemCarrinho(
	[FromServices] IRemoveItemCarrinhoUseCase useCase,
	[FromRoute] long itemId,
	[FromQuery] long usuarioId)
	{
		await useCase.ExecuteAsync(itemId, usuarioId);

		return NoContent();
	}
}
