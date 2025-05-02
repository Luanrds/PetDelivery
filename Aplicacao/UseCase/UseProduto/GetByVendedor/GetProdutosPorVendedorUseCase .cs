using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseProduto.GetByVendedor;
public class GetProdutosPorVendedorUseCase : IGetProdutosPorVendedorUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IProdutoReadOnly _produtoRepository;
	private readonly IMapper _mapper;

	public GetProdutosPorVendedorUseCase(
		IUsuarioLogado usuarioLogado,
		IProdutoReadOnly produtoRepository,
		IMapper mapper)
	{
		_usuarioLogado = usuarioLogado;
		_produtoRepository = produtoRepository;
		_mapper = mapper;
	}
	public async Task<IEnumerable<ResponseProdutoJson>> ExecuteAsync()
	{
		Usuario vendedorLogado = await _usuarioLogado.Usuario();

		IEnumerable<Produto> produtos = await _produtoRepository.GetByUsuarioIdAsync(vendedorLogado.Id);

		return _mapper.Map<IEnumerable<ResponseProdutoJson>>(produtos);
	}
}
