using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Carrinho;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.Carrinho.Buscar;

public class ObterCarrinhoUseCase : IObterCarrinhoUseCase
{
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado;

	public ObterCarrinhoUseCase(
		ICarrinhoReadOnly carrinhoReadOnly,
		IMapper mapper,
		IUsuarioLogado usuarioLogado)
	{
		_carrinhoReadOnly = carrinhoReadOnly;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
	}

	public async Task<ResponseCarrinhoDeComprasJson> ExecuteAsync()
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		CarrinhoDeCompras carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo(usuarioLogado.Id) ?? 
			throw new NotFoundException("Nenhum carrinho ativo encontrado para este usuário.");

		return _mapper.Map<ResponseCarrinhoDeComprasJson>(carrinho);
	}
}
