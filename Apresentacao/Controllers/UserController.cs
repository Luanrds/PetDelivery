using Aplicacao.UseCase.UserUseCase;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;
public class UserController : PetDeliveryBaseController
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseUsuarioRegistradoJson), StatusCodes.Status201Created)]
	public async Task<IActionResult> Register(
		[FromServices] IRegistroDeUsuarioUseCase useCase,
		[FromBody] RequestRegistroDeUsuarioJson request)
	{
		var result = await useCase.Execute(request);

		return Created(string.Empty, result);
	}
}
