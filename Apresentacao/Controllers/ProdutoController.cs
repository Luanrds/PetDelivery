using Aplicacao.Facades;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;

public class ProdutoController : PetDeliveryBaseController
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> CrieProdutoAsync(
		[FromServices] IProdutoFacade facade,
		[FromBody] RequestProdutoJson request)
	{
		var response = await facade.CrieProduto(request);

		return Created(string.Empty, response);
	}

	[HttpGet]
	[Route("{id}")]
	[ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ObtenhaProdutoPeloId(
		[FromServices] IProdutoFacade facade,
		[FromRoute] long id)
	{
		var response = await facade.ObtenhaProdutoPeloId(id);

		return Ok(response);
	}

	[HttpGet]
	[ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]

	public async Task<IActionResult> ObtenhaProdutos(
		[FromServices] IProdutoFacade facade)
	{
		var resposta = await facade.ObtenhaProduto();

		return Ok(resposta);
	}

	[HttpDelete]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ExcluirProduto(
		[FromServices] IProdutoFacade facade,
		[FromRoute] long id)
	{
		await facade.ExcluirProduto(id);

		return NoContent();
	}

	[HttpPut]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> AtualizeProduto(
		[FromServices] IProdutoFacade facade,
		[FromBody] RequestProdutoJson requisicao,
		[FromRoute] long id)
	{
		await facade.Atualize(id, requisicao);

		return NoContent();
	}
}