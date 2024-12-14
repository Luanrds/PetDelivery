using Aplicacao.Validadores;
using Dominio.DTOs;
using Dominio.Entidades;
using FluentValidation.Results;
using Infrastucture.Repositorio.Repositorios;

namespace Aplicacao.Fachadas.UseProduto;

public class ProdutoFacade
{
    private readonly RepositoryProduct _repositoryProduct;

    public ProdutoFacade(RepositoryProduct repositoryProduct)
    {
        _repositoryProduct = repositoryProduct;
    }

    public async Task<bool> CriarProduto(DTOProdutos produtoDto)
    {
        var produto = new Produto
        {
            Nome = produtoDto.Nome,
            Valor = produtoDto.Valor,
            Disponivel = produtoDto.Disponivel,
            Descricao = produtoDto.Descricao
        };

        var validationResult = Validate(produto);

        if (validationResult.IsValid)
        {
            return await _repositoryProduct.Add(produto);
        }

        return false;
    }

    public async Task<bool> AtualizarProduto(DTOProdutos produtoDto)
    {
        if (produtoDto.Id == null || produtoDto.Valor <= 0)
        {
            return false;
        }

        var produto = new Produto
        {
            Id = produtoDto.Id.Value,
            Nome = produtoDto.Nome,
            Valor = produtoDto.Valor,
            Disponivel = produtoDto.Disponivel,
            Descricao = produtoDto.Descricao
        };

        return await _repositoryProduct.Update(produto);
    }

    private static ValidationResult Validate(Produto produto)
    {
        ProdutoValidator validator = new();
        return validator.Validate(produto);
    }
}
