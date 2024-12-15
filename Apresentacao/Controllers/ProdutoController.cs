using Aplicacao.Fachadas.UseProduto;
using Microsoft.AspNetCore.Mvc;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace PetDelivery.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly ProdutoFacade _produtoFacade;

    public ProdutoController(ProdutoFacade produtoFacade)
    {
        _produtoFacade = produtoFacade;
    }

    //[HttpPost]
    //public async Task<IActionResult> CriarProduto([FromBody] DTOProdutos produto)
    //{
    //    var resultado = await _produtoFacade.CriarProduto(produto);
    //    if (resultado)
    //    {
    //        return Ok("Produto criado com sucesso!");
    //    }
    //    return BadRequest("Falha ao criar o produto.");
    //}

    [HttpPost]
    [ProducesResponseType(typeof(ResponseProdutoJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrosJson), StatusCodes.Status204NoContent)]
    public IActionResult CrieProduto([FromBody] RequestProdutoJson request)
    {
        var response = _produtoFacade.CrieProduto(request);
        return Created(string.Empty, response);
    }


    //[HttpPut]
    //public async Task<IActionResult> AtualizarProduto([FromBody] DTOProdutos produtoDto)
    //{
    //    var resultado = await _produtoFacade.AtualizarProduto(produtoDto);
    //    if (resultado)
    //    {
    //        return Ok("Produto atualizado com sucesso!");
    //    }
    //    return BadRequest("Falha ao atualizar o produto.");
    //}
}




//public async Task<IActionResult> CriarProduto([FromBody] Produto produto)
//{
//    var validationResult = await _produtoService.AddProduct(produto);

//    if (!validationResult.IsValid)
//    {
//        var erros = validationResult.Errors.Select(e => new
//        {
//            Campo = e.PropertyName,
//            Erro = e.ErrorMessage
//        });

//        return BadRequest(erros);
//    }

//    return Ok(new
//    {
//        Mensagem = "Produto criado com sucesso!",
//        Produto = produto
//    });
//}
