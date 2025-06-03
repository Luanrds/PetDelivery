using Aplicacao.UseCase.Dashboard.VendasHoje;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.API.Atributos;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;
public class DashboardController : PetDeliveryBaseController
{
	[HttpGet]
	[Route("vendas-hoje")]
	[ProducesResponseType(typeof(ResponseVendasHojeJson), StatusCodes.Status200OK)]
	[UsuarioAutenticado(requerVendedor: true)]
	public async Task<IActionResult> GetVendasHoje(
		 [FromServices] IObterVendasHojeUseCase useCase)
	{
		var response = await useCase.ExecuteAsync();
		return Ok(response);
	}
}
