using AutoMapper;
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
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.PrecoUnitario, opt => opt.Ignore())
			.ForMember(dest => dest.Carrinho, opt => opt.Ignore())
			.ForMember(dest => dest.Produto, opt => opt.Ignore());
	}

    private void DomainToResponse()
    {
        CreateMap<Dominio.Entidades.Produto, ResponseProdutoJson>()
            .ForMember(dest => dest.Id, config => config.MapFrom(source => source.Id));

		CreateMap<Dominio.Entidades.ItemCarrinhoDeCompra, ResponseCarrinhoDeComprasJson>()
		   .ForMember(dest => dest.Id, config => config.MapFrom(source => source.Id))
		   .ForMember(dest => dest.Produto, config => config.MapFrom(source => source.Produto))
		   .ForMember(dest => dest.Quantidade, config => config.MapFrom(source => source.Quantidade))
		   .ForMember(dest => dest.SubTotal, config => config.MapFrom(source => source.CalcularSubTotal()));

		CreateMap<Dominio.Entidades.CarrinhoDeCompras, List<ResponseCarrinhoDeComprasJson>>()
			.ConvertUsing((source, destination, context) =>
			{
				return source.ItensCarrinho
					.Select(item => context.Mapper.Map<ResponseCarrinhoDeComprasJson>(item))
					.ToList();
			});
	}
}
