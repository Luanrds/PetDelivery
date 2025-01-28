using Aplicacao.UseCase.Carrinho;
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
		var resposta = await useCase.Execute(request);

		return Created(string.Empty, resposta);
	}

	//[HttpGet]
	//[Route("")]
	//[ProducesResponseType(typeof(ResponseCarrinhoDeComprasJson), StatusCodes.Status200OK)]
	//[ProducesResponseType(StatusCodes.Status404NotFound)]
	//public async Task<IActionResult> ObterCarrinhoAtivo(
	//	[FromServices] IObterCarrinhoUseCase useCase)
	//{
	//	var carrinho = await useCase.ObterCarrinhoAtivo();

	//	return Ok(carrinho);
	//}

}
