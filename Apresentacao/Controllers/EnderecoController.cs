using Aplicacao.UseCase.UseEndereco.Atualizar;
using Aplicacao.UseCase.UseEndereco.Buscar;
using Aplicacao.UseCase.UseEndereco.Criar;
using Aplicacao.UseCase.UseEndereco.Excluir;
using Aplicacao.UseCase.UseProduto.Excluir;
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

	[HttpGet("{usuarioId}")]
	[ProducesResponseType(typeof(IEnumerable<ResponseEnderecoJson>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> BuscarEnderecos(
	[FromServices] IBuscarEnderecosUseCase useCase,
	[FromRoute] long usuarioId)
	{
		IEnumerable<ResponseEnderecoJson> response = await useCase.Execute(usuarioId);

		return Ok(response);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Atualize(
		[FromServices] IAtualizeEnderecoUseCase useCase,
		[FromBody] RequestAtualizarEnderecoJson request,
		[FromRoute] long id)
	{
		await useCase.Execute(id, request);

		return NoContent();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ExcluirEndereco(
		[FromServices] IExcluirEnderecoUseCase useCase,
		[FromRoute] long id)
	{
		await useCase.Execute(id);

		return NoContent();
	}
}
