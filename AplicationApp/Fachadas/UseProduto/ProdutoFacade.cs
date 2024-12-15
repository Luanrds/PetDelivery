using Aplicacao.Servicos;
using Aplicacao.Validadores;
using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.Fachadas.UseProduto;

public class ProdutoFacade
{
    private readonly IProdutoReadOnly _readOnly;
    private readonly IProdutoWriteOnly _writeOnly;

    //public async Task<bool> CriarProduto(DTOProdutos produtoDto)
    //{
    //    var produto = new Produto
    //    {
    //        Nome = produtoDto.Nome,
    //        Valor = produtoDto.Valor,
    //        Disponivel = produtoDto.Disponivel,
    //        Descricao = produtoDto.Descricao
    //    };

    //    var validationResult = Validate(produto);

    //    if (validationResult.IsValid)
    //    {
    //        return await _repositoryProduct.Add(produto);
    //    }

    //    return false;
    //}

    public async Task<ResponseProdutoJson> CrieProduto(RequestProdutoJson request)
    {
        Validate(request);

        var autoMapper = new AutoMapper.MapperConfiguration(options =>
        {
            options.AddProfile(new AutoMapping());
        }).CreateMapper();

        var produto = autoMapper.Map<Produto>(request);

        await _writeOnly.Add(produto);

        return new ResponseProdutoJson
        {
            Descricao = request.Descricao,
            Disponivel = request.Disponivel,
            Nome = request.Nome,
            Valor = request.Valor
        };
    }

    //public async Task<bool> AtualizarProduto(DTOProdutos produtoDto)
    //{
    //    if (produtoDto.Id == null || produtoDto.Valor <= 0)
    //    {
    //        return false;
    //    }

    //    var produto = new Produto
    //    {
    //        Id = produtoDto.Id.Value,
    //        Nome = produtoDto.Nome,
    //        Valor = produtoDto.Valor,
    //        Disponivel = produtoDto.Disponivel,
    //        Descricao = produtoDto.Descricao
    //    };

    //    return await _repositoryProduct.Update(produto);
    //}

    //private static ValidationResult Validate(Produto produto)
    //{
    //    ProdutoValidator validator = new();
    //    return validator.Validate(produto);
    //}

    private static void Validate(RequestProdutoJson request)
    {
        var validator = new ProdutoValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var mensagensDeErro = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(mensagensDeErro);
        }
    }
}
