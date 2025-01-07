using Aplicacao.UseCase.UseProduto;
using Aplicacao.UseCase.UseProduto.GetById;
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
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> GetbyId(
		[FromServices] IGetProdutoById produtoUseCase,
		[FromRoute] [ModelBinder(typeof(PetDeliveryBinder))] long id)
	{
		var response = await produtoUseCase.Execute(id);

		return Ok(response);
	}
}