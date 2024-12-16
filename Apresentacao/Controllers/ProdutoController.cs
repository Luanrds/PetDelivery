using Aplicacao.Fachadas.UseProduto;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
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
}