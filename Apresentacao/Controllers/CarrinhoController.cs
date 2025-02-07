using Aplicacao.UseCase.Carrinho;
using Aplicacao.UseCase.Carrinho.Atualizar;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;
public class CarrinhoController : PetDeliveryBaseController
{
	[HttpPost]
	[Route("item")]
	[ProducesResponseType(typeof(ResponseCarrinhoDeComprasJson), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> AdicioneItemCarrinho(
		[FromServices] ICarrinhoUseCase useCase,
		[FromBody] RequestItemCarrinhoJson request)
	{
		var resposta = await useCase.Execute(request);

		return Created(string.Empty, resposta);
	}

    [HttpPut]
    [Route("item/{itemId}")]
    [ProducesResponseType(typeof(ResponseCarrinhoDeComprasJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AtualizarQuantidadeItemCarrinho(
        [FromServices] IAtualizeQtdItemCarrinhoUseCase useCase,
        [FromRoute] long itemId,
        [FromBody] RequestAtualizarItemCarrinhoJson request)
    {
        var resposta = await useCase.AtualizeQuantidade(itemId, request);

        return Ok(resposta);
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterCarrinho(
        [FromServices] IObterCarrinhoUseCase useCase)
    {
        var resposta = await useCase.Executar();

        return Ok(resposta);
    }
}
