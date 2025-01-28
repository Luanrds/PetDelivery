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
	public async Task<IActionResult> CrieProdutoAsync(
		[FromServices] IProdutoUseCase produtoUseCase,
		[FromBody] RequestProdutoJson request)
	{
		var response = await produtoUseCase.CrieProduto(request);

		return Created(string.Empty, response);
	}

	[HttpGet]
	[Route("{id}")]
	[ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetbyId(
		[FromServices] IGetProdutoById produtoUseCase,
		[FromRoute] long id)
	{
		var response = await produtoUseCase.Execute(id);

		return Ok(response);
	}

	[HttpGet]
	[ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]

	public async Task<IActionResult> ObtenhaProdutos(
		[FromServices] IObtenhaTodosProdutos useCase)
	{
		var resposta = await useCase.Execute();

		return Ok(resposta);
	}

	[HttpDelete]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ExcluirProduto(
		[FromServices] IExcluirProdutoUseCase produtoUseCase,
		[FromRoute] long id)
	{
		await produtoUseCase.Execute(id);

		return NoContent();
	}

	[HttpPut]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> AtualizeProduto(
		[FromServices] IAtualizeProdutoUseCase produtoUseCase,
		[FromBody] RequestProdutoJson requisicao,
		[FromRoute] long id)
	{
		await produtoUseCase.Execute(id, requisicao);

		return NoContent();
	}
}