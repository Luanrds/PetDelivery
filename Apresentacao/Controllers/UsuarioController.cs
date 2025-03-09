﻿using Aplicacao.UseCase.UseUsuario.AlterarSenha;
using Aplicacao.UseCase.UseUsuario.Atualizar;
using Aplicacao.UseCase.UseUsuario.Buscar;
using Aplicacao.UseCase.UseUsuario.Criar;
using Aplicacao.UseCase.UseUsuario.Excluir;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;
public class UsuarioController : PetDeliveryBaseController
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseUsuarioJson), StatusCodes.Status200OK)]
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

	[HttpPut("{id}")]
	[ProducesResponseType(typeof(ResponseUsuarioJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Atualizar(
	[FromServices] IAtualizarUsuarioUseCase atualizarUsuarioUseCase,
	[FromRoute] long id,
	[FromBody] RequestAtualizarUsuarioJson request)
	{
		await atualizarUsuarioUseCase.Execute(id, request);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Deletar(
			[FromServices] IExcluirUsuarioUseCase excluirUsuarioUseCase,
			[FromRoute] long id)
	{
		await excluirUsuarioUseCase.Execute(id);

		return NoContent();
	}

	[HttpPut("{id}/senha")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> AlterarSenha(
		[FromServices] IAlterarSenhaUsuarioUseCase alterarSenhaUsuarioUseCase,
		[FromRoute] long id,
		[FromBody] RequestAlterarSenhaUsuarioJson request)
	{
		await alterarSenhaUsuarioUseCase.Execute(id, request);

		return NoContent();
	}
}
