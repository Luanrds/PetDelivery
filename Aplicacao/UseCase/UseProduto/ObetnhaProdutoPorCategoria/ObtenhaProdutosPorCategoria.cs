using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.ObetnhaProdutoPorCategoria;

public class ObtenhaProdutosPorCategoriaUseCase(IProdutoReadOnly produtoReadOnlyRepositorio, IMapper mapper) : IObtenhaProdutosPorCategoria
{
	private readonly IProdutoReadOnly _produtoReadOnly = produtoReadOnlyRepositorio;
	private readonly IMapper _mapper = mapper;

	public async Task<IEnumerable<ResponseProdutoJson>> Execute(string categoria)
	{
		IEnumerable<Produto> produtos = 
			await _produtoReadOnly.ObterPorCategoria(categoria);

		IEnumerable<ResponseProdutoJson> response = 
			_mapper.Map<IEnumerable<ResponseProdutoJson>>(produtos);

		return response;
	}
}