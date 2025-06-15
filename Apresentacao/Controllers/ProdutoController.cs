using Aplicacao.UseCase.UseProduto.Atualizar;
using Aplicacao.UseCase.UseProduto.Buscar;
using Aplicacao.UseCase.UseProduto.Criar;
using Aplicacao.UseCase.UseProduto.Desconto;
using Aplicacao.UseCase.UseProduto.Excluir;
using Aplicacao.UseCase.UseProduto.GetById;
using Aplicacao.UseCase.UseProduto.GetByVendedor;
using Aplicacao.UseCase.UseProduto.Imagem;
using Aplicacao.UseCase.UseProduto.ObetnhaProdutoPorCategoria;
using Aplicacao.UseCase.UseProduto.ObtenhaTodosProdutos;
using Aplicacao.UseCase.UseProduto.ObterEmPromocao;
using Aplicacao.UseCase.UseProduto.ObterMaisVendidos;
using Aplicacao.UseCase.UseProduto.ObterRelacionados;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.API.Atributos;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;

public class ProdutoController : PetDeliveryBaseController
{
	[HttpPost]
	[ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status204NoContent)]
	[UsuarioAutenticado(requerVendedor: true)]
	public async Task<IActionResult> CrieProduto(
		[FromServices] IProdutoUseCase useCase,
		[FromForm] RequestRegistroProdutoFormData request)
	{
		ResponseProdutoJson response = await useCase.ExecuteAsync(request);

		return Created(string.Empty, response);
	}

	[HttpGet]
	[Route("{id}")]
	[ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> GetbyId(
		[FromServices] IGetProdutoById useCase,
		[FromRoute] long id)
	{
		ResponseProdutoJson response = await useCase.ExecuteAsync(id);

		return Ok(response);
	}

	[HttpGet]
	[Route("produtos")]
	[ProducesResponseType(typeof(ResponseProdutosJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status204NoContent)]
	public async Task<IActionResult> ObtenhaProdutos(
		[FromServices] IObtenhaTodosProdutos useCase)
	{
		ResponseProdutosJson response = await useCase.ExecuteAsync();

		if (response.Produtos.Any())
		{
			return Ok(response);
		}

		return NoContent();
	}

	[HttpGet("meus-produtos")]
	[ProducesResponseType(typeof(ResponseProdutosJson), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[UsuarioAutenticado(requerVendedor: true)]
	public async Task<IActionResult> GetMeusProdutos(
		[FromServices] IGetProdutosPorVendedorUseCase useCase)
	{
		ResponseProdutosJson response = await useCase.ExecuteAsync();

		if (response.Produtos.Any())
		{
			return Ok(response);
		}

		return NoContent();
	}

	[HttpGet]
	[Route("categoria/{categoria}")]
	[ProducesResponseType(typeof(IEnumerable<ResponseProdutoJson>), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> ObtenhaProdutosPorCategoria(
		[FromServices] IObtenhaProdutosPorCategoria useCase,
		[FromRoute] string categoria)
	{
		IEnumerable<ResponseProdutoJson> resposta = await useCase.ExecuteAsync(categoria);

		if (resposta == null || !resposta.Any())
		{
			return NoContent();
		}

		return Ok(resposta);
	}

	[HttpGet("buscar")]
	[ProducesResponseType(typeof(ResponseProdutosJson), StatusCodes.Status200OK)]
	public async Task<IActionResult> Buscar(
	[FromServices] IBuscarProdutosUseCase useCase,
	[FromQuery] RequestBuscaProdutosJson request)
	{
		ResponseProdutosJson response = await useCase.Execute(request);
		return Ok(response);
	}

	[HttpGet("em-promocao")]
	[ProducesResponseType(typeof(ResponseProdutosJson), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	public async Task<IActionResult> ObterPromocoes(
		[FromServices] IObterProdutosEmPromocaoUseCase useCase,
		[FromQuery] int pagina = 1,
		[FromQuery] int itensPorPagina = 10)
	{
		var response = await useCase.ExecuteAsync(pagina, itensPorPagina);

		if (response.Produtos.Count == 0)
		{
			return NotFound();
		}

		return Ok(response);
	}

	[HttpGet("{id}/relacionados")]
	[ProducesResponseType(typeof(ResponseProdutosJson), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> ObterRelacionados(
		[FromServices] IObterProdutosRelacionadosUseCase useCase,
		[FromRoute] long id,
		[FromQuery] int itensPorPagina = 4)
	{
		var response = await useCase.ExecuteAsync(id, itensPorPagina);

		if (response.Produtos.Count == 0)
		{
			return NoContent();
		}

		return Ok(response);
	}

	[HttpGet("mais-vendidos")]
	[ProducesResponseType(typeof(ResponseProdutosJson), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	public async Task<IActionResult> ObterMaisVendidos(
		[FromServices] IObterMaisVendidosUseCase useCase,
		[FromQuery] int limite = 10)
	{
		var response = await useCase.ExecuteAsync(limite);

		if (response.Produtos.Count == 0)
		{
			return NoContent();
		}

		return Ok(response);
	}

	[HttpDelete]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	[UsuarioAutenticado(requerVendedor: true)]
	public async Task<IActionResult> ExcluirProduto(
		[FromServices] IExcluirProdutoUseCase useCase,
		[FromRoute] long id)
	{
		await useCase.ExecuteAsync(id);

		return NoContent();
	}

	[HttpPut]
	[Route("{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	[UsuarioAutenticado(requerVendedor: true)]
	public async Task<IActionResult> AtualizeProduto(
		[FromServices] IAtualizeProdutoUseCase useCase,
		[FromForm] RequestRegistroProdutoFormData requisicao,
		[FromRoute] long id)
	{
		await useCase.ExecuteAsync(id, requisicao);

		return NoContent();
	}

	[HttpPut]
	[Route("image/{id}")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	[UsuarioAutenticado(requerVendedor: true)]
	public async Task<IActionResult> AdicionarImagemProduto(
		[FromServices] IAddUpdateImageCoverUseCase useCase,
		[FromRoute] long id,
		IFormFile file)
	{
		await useCase.Execute(id, file);

		return NoContent();
	}

	[HttpPut]
	[Route("{id}/desconto")]
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
	[UsuarioAutenticado(requerVendedor: true)]
	public async Task<IActionResult> AplicarDesconto(
		[FromServices] IAplicarDescontoUseCase useCase,
		[FromRoute] long id,
		[FromBody] RequestAplicarDescontoJson request)
	{
		await useCase.Execute(id, request);
		return NoContent();
	}
}