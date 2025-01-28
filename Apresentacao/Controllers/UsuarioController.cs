using Aplicacao.UseCase.UseUsuario.BuscarPorEmail;
using Aplicacao.UseCase.UseUsuario.Criar;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;
public class UsuarioController : PetDeliveryBaseController
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

	[HttpGet]
	[Route("{id}")]
	[ProducesResponseType(typeof(ResponsePerfilUsuario), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetUserProfile(
		[FromServices] IBucarPerfilUsuarioUseCase useCase,
		[FromRoute] long id)
	{
		var result = await useCase.Execute(id);

		return Ok(result);
	}
}
