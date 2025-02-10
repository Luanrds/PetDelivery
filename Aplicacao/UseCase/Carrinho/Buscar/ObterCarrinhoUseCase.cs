using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Carrinho;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Carrinho.Buscar;
public class ObterCarrinhoUseCase : IObterCarrinhoUseCase
{
	private readonly ICarrinhoReadOnly _carrinhoReadOnly;
	private readonly IMapper _mapper;

	public ObterCarrinhoUseCase(ICarrinhoReadOnly carrinhoReadOnly, IMapper mapper)
	{
		_carrinhoReadOnly = carrinhoReadOnly;
		_mapper = mapper;
	}

	public async Task<ResponseCarrinhoDeComprasJson> Execute()
	{
		var carrinho = await _carrinhoReadOnly.ObtenhaCarrinhoAtivo();

		if (carrinho == null) 
			_ = new CarrinhoDeCompras();

		return _mapper.Map<ResponseCarrinhoDeComprasJson>(carrinho);
	}
}
