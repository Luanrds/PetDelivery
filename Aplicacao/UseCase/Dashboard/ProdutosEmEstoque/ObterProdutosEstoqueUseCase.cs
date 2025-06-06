using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Dashboard.ProdutosEmEstoque;
public class ObterProdutosEstoqueUseCase : IObterProdutosEstoqueUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IProdutoReadOnly _produtoReadOnly;

	public ObterProdutosEstoqueUseCase(IUsuarioLogado usuarioLogado, IProdutoReadOnly produtoReadOnly)
	{
		_usuarioLogado = usuarioLogado;
		_produtoReadOnly = produtoReadOnly;
	}

	public async Task<ResponseProdutosEstoqueJson> ExecuteAsync()
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		int estoqueAtual = await _produtoReadOnly.GetTotalEstoquePorVendedorAsync(usuarioLogado.Id);

		return new ResponseProdutosEstoqueJson
		{
			TotalProdutosEmEstoque = estoqueAtual
		};
	}
}
