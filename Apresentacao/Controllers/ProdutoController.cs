using Aplicacao.UseCase.UseProduto.Atualizar;
using Aplicacao.UseCase.UseProduto.Criar;
using Aplicacao.UseCase.UseProduto.Excluir;
using Aplicacao.UseCase.UseProduto.GetById;
using Aplicacao.UseCase.UseProduto.ObtenhaTodosProdutos;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;

public class ProdutoController : PetDeliveryBaseController
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> CrieProduto(
		[FromServices] IProdutoUseCase useCase,
		[FromBody] RequestProdutoJson request)
	{
		ResponseProdutoJson response = await useCase.Execute(request);

		return Created(string.Empty, response);
	}

	[HttpGet]
	[Route("{id}")]
	[ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetbyId(
		[FromServices] IGetProdutoById useCase,
		[FromRoute] long id)
	{
		ResponseProdutoJson response = await useCase.Execute(id);

		return Ok(response);
	}

	[HttpGet]
	[Route("produtos")]
	[ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]

	public async Task<IActionResult> ObtenhaProdutos(
		[FromServices] IObtenhaTodosProdutos useCase)
	{
		IEnumerable<ResponseProdutoJson> response = await useCase.Execute();

		return Ok(response);
	}

	[HttpDelete]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ExcluirProduto(
		[FromServices] IExcluirProdutoUseCase useCase,
		[FromRoute] long id)
	{
		await useCase.Execute(id);

		return NoContent();
	}

	[HttpPut]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> AtualizeProduto(
		[FromServices] IAtualizeProdutoUseCase useCase,
		[FromBody] RequestProdutoJson requisicao,
		[FromRoute] long id)
	{
		await useCase.Execute(id, requisicao);

		return NoContent();
	}
}