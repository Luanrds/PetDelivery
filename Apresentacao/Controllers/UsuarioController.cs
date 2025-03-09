using Aplicacao.UseCase.UseUsuario.Buscar;
using Aplicacao.UseCase.UseUsuario.Criar;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;
public class UsuarioController : PetDeliveryBaseController
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseUsuarioRegistradoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> Registre(
	[FromServices] IUsuarioUseCase usuarioUseCase,
	[FromBody] RequestUsuarioRegistroJson request)
	{
		var resposta = await usuarioUseCase.Execute(request);

		return Created(string.Empty, resposta);
	}

	[HttpGet("{id}")]
	[ProducesResponseType(typeof(ResponseUsuarioJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Obter(
	[FromServices] IObterUsuarioUseCase obterUsuarioUseCase,
	[FromRoute] long id)
	{
		var resposta = await obterUsuarioUseCase.Execute(id);

		return Ok(resposta);
	}
}
