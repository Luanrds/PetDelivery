using Aplicacao.UseCase.UserUseCase;
using Microsoft.AspNetCore.Mvc;

namespace PetDelivery.API.Controllers;
public class UserController : PetDeliveryBaseController
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
	public async Task<IActionResult> Register(
		[FromServices] IRegistroDeUsuarioUseCase useCase,
		[FromBody] RegistroDeUsuarioUseCase request)
	{
		var result = await useCase.Execute(request);

		return Created(string.Empty, result);
	}
}
