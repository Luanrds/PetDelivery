using AutoMapper;
using Dominio.Entidades;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.Servicos;

public class AutoMapping : Profile
{
	public AutoMapping()
	{
		RequestToDomain();
		DomainToResponse();

	}

	private void RequestToDomain()
	{
		CreateMap<RequestProdutoJson, Dominio.Entidades.Produto>()
			.ForMember(dest => dest.Id, opt => opt.Ignore());

		CreateMap<RequestItemCarrinhoJson, Dominio.Entidades.ItemCarrinhoDeCompra>()
			.ForMember(dest => dest.Id, opt => opt.Ignore()) // Ignorar o Id do ItemCarrinho
			.ForMember(dest => dest.PrecoUnitario, opt => opt.Ignore()) // Ignorar preço unitário, que é calculado
			.ForMember(dest => dest.Carrinho, opt => opt.Ignore()) // Ignorar Carrinho, pois é atribuído posteriormente
			.ForMember(dest => dest.ProdutoId, opt => opt.MapFrom(src => src.ProdutoId)); // Associar ProdutoId
	}

	private void DomainToResponse()
	{
		CreateMap<Dominio.Entidades.Produto, ResponseProdutoJson>()
			.ForMember(dest => dest.Id, config => config.MapFrom(source => source.Id));

		CreateMap<ItemCarrinhoDeCompra, ResponseItemCarrinhoJson>()
			.ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.CalcularSubTotal()));

		CreateMap<CarrinhoDeCompras, ResponseCarrinhoDeComprasJson>()
				.ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.ItensCarrinho)) // Mapear os itens do carrinho
				.ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.ItensCarrinho.Sum(i => i.CalcularSubTotal()))); // Calcular o Total
	}
}
