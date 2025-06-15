using Aplicacao.UseCase.Pagamento.Criar;
using Aplicacao.UseCase.Pagamento.Excluir;
using Aplicacao.UseCase.Pagamento.Listar;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.API.Atributos;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;

[UsuarioAutenticado]
public class MetodoPagamentoController : PetDeliveryBaseController
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseCartaoCreditoJson), StatusCodes.Status201Created)]
	public async Task<IActionResult> RegistrarCartao(
		[FromServices] ICriarMetodoPagamentoUseCase useCase,
		[FromBody] RequestCartaoCreditoJson request)
	{
		var response = await useCase.Execute(request);
		return Created(string.Empty, response);
	}

	[HttpGet]
	[ProducesResponseType(typeof(IList<ResponseCartaoCreditoJson>), StatusCodes.Status200OK)]
	public async Task<IActionResult> ListarCartoes(
		[FromServices] IListarMetodosPagamentoUseCase useCase)
	{
		var response = await useCase.Execute();
		return Ok(response);
	}

	[HttpDelete("{id:long}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> ExcluirCartao(
		[FromServices] IExcluirMetodoPagamentoUseCase useCase,
		[FromRoute] long id)
	{
		await useCase.Execute(id);
		return NoContent();
	}
}
