using Aplicacao.Extensoes;
using AutoMapper;
using Dominio.Repositorios.Carrinho;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Carrinho.Buscar;

public class ObterCarrinhoUseCase : IObterCarrinhoUseCase
{
	private readonly ICarrinhoReadOnly _readOnlyRepository;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IMapper _mapper;
	private readonly IBlobStorageService _blobStorageService;

	public ObterCarrinhoUseCase(
		ICarrinhoReadOnly readOnlyRepository,
		IUsuarioLogado usuarioLogado,
		IMapper mapper,
		IBlobStorageService blobStorageService)
	{
		_readOnlyRepository = readOnlyRepository;
		_usuarioLogado = usuarioLogado;
		_mapper = mapper;
		_blobStorageService = blobStorageService;
	}

	public async Task<ResponseCarrinhoDeComprasJson> ExecuteAsync()
	{
		var usuario = await _usuarioLogado.Usuario();
		Dominio.Entidades.CarrinhoDeCompras? carrinho = await _readOnlyRepository.ObtenhaCarrinhoAtivo(usuario.Id);

		if (carrinho == null || carrinho.ItensCarrinho == null || !carrinho.ItensCarrinho.Any())
		{
			return new ResponseCarrinhoDeComprasJson
			{
				Id = carrinho?.Id ?? 0,
				Itens = [],
				Total = 0
			};
		}

		ResponseCarrinhoDeComprasJson response = _mapper.Map<ResponseCarrinhoDeComprasJson>(carrinho);

		response.Itens = await carrinho.ItensCarrinho.MapToResponseItensCarrinhoJson(usuario, _blobStorageService, _mapper);
		response.Total = carrinho.ItensCarrinho.Sum(item => item.CalcularSubTotal());

		return response;
	}
}