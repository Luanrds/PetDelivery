using AutoMapper;
using Dominio.Entidades;
using Dominio.Enums;
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
		CreateMap<RequestUsuarioRegistroJson, Usuario>()
			.ForMember(dest => dest.Id, opt => opt.Ignore());

		CreateMap<RequestAtualizarUsuarioJson, Usuario>()
			.ForMember(dest => dest.Id, opt => opt.Ignore());

		CreateMap<RequestEnderecoJson, Endereco>()
					.ForMember(dest => dest.Id, opt => opt.Ignore())
					.ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId));

		CreateMap<RequestAtualizarEnderecoJson, Endereco>()
			.ForMember(dest => dest.Id, opt => opt.Ignore());

		CreateMap<RequestProdutoJson, Produto>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.CategoriaProduto, opt => opt.MapFrom(src => (CategoriaProduto)src.CategoriaProduto));

		CreateMap<RequestItemCarrinhoJson, ItemCarrinhoDeCompra>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.ProdutoId, opt => opt.MapFrom(src => src.ProdutoId));

		CreateMap<RequestAtualizarItemCarrinhoJson, ItemCarrinhoDeCompra>()
			.ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.Quantidade));
	}

	private void DomainToResponse()
	{
		CreateMap<Usuario, ResponseUsuarioJson>();

		CreateMap<Endereco, ResponseEnderecoJson>();

		CreateMap<Produto, ResponseProdutoJson>()
			.ForMember(dest => dest.Id, config => config.MapFrom(source => source.Id))
			.ForMember(dest => dest.CategoriaProduto, opt => opt.MapFrom(src => src.CategoriaProduto.ToString()));

		CreateMap<ItemCarrinhoDeCompra, ResponseItemCarrinhoJson>()
			.ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.CalcularSubTotal()));

		CreateMap<CarrinhoDeCompras, ResponseCarrinhoDeComprasJson>()
			.ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.ItensCarrinho)) // Mapear os itens do carrinho
			.ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.ItensCarrinho.Sum(i => i.CalcularSubTotal()))); // Calcular o Total
	}
}