using Aplicacao.Extensoes;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Carrinho;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.Carrinho.Buscar;

public class ObterCarrinhoUseCase : IObterCarrinhoUseCase
{
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IBlobStorageService _blobStorageService;

	public ObterCarrinhoUseCase(
		ICarrinhoReadOnly carrinhoReadOnly,
		IMapper mapper,
		IUsuarioLogado usuarioLogado,
		IBlobStorageService blobStorageService)
	{
		_carrinhoReadOnly = carrinhoReadOnly;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
		_blobStorageService = blobStorageService;
	}

	public async Task<ResponseCarrinhoDeComprasJson> ExecuteAsync()
	{
		Usuario usuarioQueEstaVendoOCarrinho = await _usuarioLogado.Usuario();

		CarrinhoDeCompras carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(usuarioQueEstaVendoOCarrinho.Id) 
			?? throw new NotFoundException("Nenhum carrinho ativo encontrado para este usuário.");

		ResponseCarrinhoDeComprasJson response = _mapper.Map<ResponseCarrinhoDeComprasJson>(carrinho);

		response.Itens = carrinho.ItensCarrinho != null && carrinho.ItensCarrinho.Count != 0
			? await carrinho.ItensCarrinho.MapToResponseItemCarrinhoJsonComImagens(_blobStorageService, _mapper)
			: ([]);

		return response;
	}
}