using Aplicacao.UseCase.Dashboard.NovosPedidosHoje;
using Aplicacao.UseCase.Dashboard.ObterUltimosPedidos;
using Aplicacao.UseCase.Dashboard.ProdutosEmEstoque;
using Aplicacao.UseCase.Dashboard.ProdutosMaisVendidos;
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
		ResponseVendasHojeJson response = await useCase.ExecuteAsync();
		return Ok(response);
	}

	[HttpGet]
	[Route("novos-pedidos-hoje")]
	[ProducesResponseType(typeof(ResponseNovosPedidosHojeJson), StatusCodes.Status200OK)]
	[UsuarioAutenticado(requerVendedor: true)]
	public async Task<IActionResult> GetNovosPedidosHoje(
		[FromServices] IObterNovosPedidosHojeUseCase useCase)
	{
		ResponseNovosPedidosHojeJson response = await useCase.ExecuteAsync();
		return Ok(response);
	}

	[HttpGet]
	[Route("produtos-em-estoque")]
	[ProducesResponseType(typeof(ResponseProdutosEstoqueJson), StatusCodes.Status200OK)]
	[UsuarioAutenticado(requerVendedor: true)]
	public async Task<IActionResult> GetProdutosEmEstoque(
	[FromServices] IObterProdutosEstoqueUseCase useCase)
	{
		var response = await useCase.ExecuteAsync();
		return Ok(response);
	}

	[HttpGet]
	[Route("produtos-mais-vendidos")]
	[ProducesResponseType(typeof(ResponseProdutosMaisVendidosJson), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[UsuarioAutenticado(requerVendedor: true)]
	public async Task<IActionResult> GetProdutosMaisVendidos(
		[FromServices] IObterProdutosMaisVendidosUseCase useCase,
		[FromQuery] int topN = 5)
	{
		var response = await useCase.ExecuteAsync(topN);
		if (response.Produtos != null && response.Produtos.Any())
		{
			return Ok(response);
		}
		return NoContent();
	}

	[HttpGet]
	[Route("ultimos-pedidos")]
	[ProducesResponseType(typeof(ResponseUltimosPedidosJson), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> GetUltimosPedidos(
	[FromServices] IObterUltimosPedidosUseCase useCase,
	[FromQuery] int topN = 5)
	{
		var response = await useCase.ExecuteAsync(topN);
		if (response.Pedidos != null && response.Pedidos.Any())
		{
			return Ok(response);
		}
		return NoContent();
	}
}
