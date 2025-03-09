using Aplicacao.UseCase.UseEndereco.Criar;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;

public class EnderecoController : PetDeliveryBaseController
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseEnderecoJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CrieEndereco(
		[FromServices] IEnderecoUseCase useCase,
		[FromBody] RequestEnderecoJson request)
	{
		var response = await useCase.Execute(request);

		return Created(string.Empty, response);
	}
}
