using AutoMapper;
using Dominio.Entidades;
using Dominio.ObjetosDeValor;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Dashboard.ProdutosMaisVendidos;
public class ObterProdutosMaisVendidosUseCase : IObterProdutosMaisVendidosUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IProdutoReadOnly _produtoReadOnly;
	private readonly IMapper _mapper;

	public ObterProdutosMaisVendidosUseCase(
		IUsuarioLogado usuarioLogado,
		IProdutoReadOnly produtoReadOnly,
		IMapper mapper)
	{
		_usuarioLogado = usuarioLogado;
		_produtoReadOnly = produtoReadOnly;
		_mapper = mapper;
	}

	public async Task<ResponseProdutosMaisVendidosJson> ExecuteAsync(int topN = 5)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		IList<ProdutoVendidoInfo> produtosInfo = await _produtoReadOnly.GetProdutosMaisVendidosPorVendedorAsync(usuarioLogado.Id, topN);

		var produtosResponse = 
			_mapper.Map<List<ResponseProdutoMaisVendidoJson>>(produtosInfo);

		return new ResponseProdutosMaisVendidosJson
		{
			Produtos = produtosResponse
		};
	}
}