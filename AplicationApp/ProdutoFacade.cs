using Dominio.Interfaces.InterfaceServices;
using Entidades.Entidades;
using Infrastucture.Configuracao;
using Infrastucture.Repositorio.Repositorios;

namespace Aplicacao
{
    public class ProdutoFacade
    {
        private readonly RepositoryProduct _repositoryProduct;

        public ProdutoFacade(RepositoryProduct repositoryProduct)
        {
            _repositoryProduct = repositoryProduct;
        }

        public async Task<bool> CriarProduto(DTOProdutos produtoDto)
        {
            if (string.IsNullOrEmpty(produtoDto.Nome) || produtoDto.Valor <= 0)
            {
                return false; 
            }

            var produto = new Produto
            {
                Nome = produtoDto.Nome,
                Valor = produtoDto.Valor,
                Disponivel = produtoDto.Disponivel,
                Descricao = produtoDto.Descricao
            };

            return await _repositoryProduct.Add(produto);
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
    }
}
