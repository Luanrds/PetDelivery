using Aplicacao.UseCase.UseUsuario.AlterarSenha;
using Aplicacao.UseCase.UseUsuario.Atualizar;
using Aplicacao.UseCase.UseUsuario.Buscar;
using Aplicacao.UseCase.UseUsuario.Criar;
using Aplicacao.UseCase.UseUsuario.Excluir;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.API.Atributos;
using PetDelivery.Communication;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;

public class UsuarioController : PetDeliveryBaseController
{
	[HttpPost("Cadastro")]
	[ProducesResponseType(typeof(ResponseUsuarioJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> Registre(
	[FromServices] IUsuarioUseCase UseCase,
	[FromBody] RequestUsuarioRegistroJson request)
	{
		ResponseUsuarioJson resposta = await UseCase.ExecuteAsync(request);

		return Created(string.Empty, resposta);
	}

	[HttpGet]
	[ProducesResponseType(typeof(ResponsePerfilUsuarioJson), StatusCodes.Status200OK)]
	[UsuarioAutenticado]
	public async Task<IActionResult> ObterPerfilUsuario(
	[FromServices] IObterPerfilUsuarioUseCase UseCase)
	{
		ResponsePerfilUsuarioJson resposta = await UseCase.ExecuteAsync();

		return Ok(resposta);
	}

	[HttpPut]
	[ProducesResponseType (StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status400BadRequest)]
	[UsuarioAutenticado]
	public async Task<IActionResult> Atualizar(
	[FromServices] IAtualizarUsuarioUseCase UseCase,
	[FromBody] RequestAtualizarUsuarioJson request)
	{
		await UseCase.ExecuteAsync(request);

		return NoContent();
	}

	[HttpPut("alterar-senha")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> AlterarSenha(
	[FromServices] IAlterarSenhaUsuarioUseCase UseCase,
	[FromBody] RequestAlterarSenhaUsuarioJson request)
	{
		await UseCase.ExecuteAsync(request);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Deletar(
			[FromServices] IExcluirUsuarioUseCase UseCase,
			[FromRoute] long id)
	{
		await UseCase.ExecuteAsync(id);

		return NoContent();
	}
}
