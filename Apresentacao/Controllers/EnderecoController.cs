using Aplicacao.UseCase.UseEndereco.Atualizar;
using Aplicacao.UseCase.UseEndereco.Buscar;
using Aplicacao.UseCase.UseEndereco.Criar;
using Aplicacao.UseCase.UseEndereco.Excluir;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.API.Atributos;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;

[UsuarioAutenticado]
public class EnderecoController : PetDeliveryBaseController
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseEnderecoJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> CrieEndereco(
		[FromServices] IEnderecoUseCase useCase,
		[FromBody] RequestEnderecoJson request)
	{
		ResponseEnderecoJson resposta = await useCase.ExecuteAsync(request);

		return Created(string.Empty, resposta);
	}

	[HttpGet]
	[ProducesResponseType(typeof(IEnumerable<ResponseEnderecoJson>), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> BuscarEnderecos(
	[FromServices] IBuscarEnderecosUseCase useCase)
	{
		IEnumerable<ResponseEnderecoJson> resposta = await useCase.ExecuteAsync();

		return Ok(resposta);
	}

	[HttpPut("endereco/{enderecoId:long}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Atualize(
		[FromServices] IAtualizeEnderecoUseCase useCase,
		[FromBody] RequestAtualizarEnderecoJson request,
		[FromRoute] long enderecoId)
	{
		await useCase.ExecuteAsync(enderecoId, request);

		return NoContent();
	}

	[HttpDelete("endereco/{enderecoId:long}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ExcluirEndereco(
		[FromServices] IExcluirEnderecoUseCase useCase,
		[FromRoute] long enderecoId)
	{
		await useCase.ExecuteAsync(enderecoId);

		return NoContent();
	}
}
