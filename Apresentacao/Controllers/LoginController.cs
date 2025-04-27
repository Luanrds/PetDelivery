using Aplicacao.UseCase.UseUsuario.Login;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;

public class LoginController : PetDeliveryBaseController
{
	[HttpPost("login")]
	[ProducesResponseType(typeof(ResponseUsuarioJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
	public async Task<IActionResult> Login(
	[FromServices] ILoginUseCase useCase,
	[FromBody] RequestLoginUsuarioJson request)
	{
		var response = await useCase.ExecuteAsync(request);

		return Ok(response);
	}
}
