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
		CreateMap<RequestUsuarioRegistroJson, Usuario>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.EhVendedor, opt => opt.MapFrom(src => src.EhVendedor));

		CreateMap<RequestAtualizarUsuarioJson, Usuario>()
			.ForMember(dest => dest.Id, opt => opt.Ignore());

		CreateMap<RequestEnderecoJson, Endereco>()
					.ForMember(dest => dest.Id, opt => opt.Ignore());

		CreateMap<RequestAtualizarEnderecoJson, Endereco>()
			.ForMember(dest => dest.Id, opt => opt.Ignore());

		CreateMap<RequestProdutoJson, Produto>()
			.ForMember(dest => dest.Id, opt => opt.Ignore());

		CreateMap<RequestItemCarrinhoJson, ItemCarrinhoDeCompra>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.ProdutoId, opt => opt.MapFrom(src => src.ProdutoId));

		CreateMap<RequestAtualizarItemCarrinhoJson, ItemCarrinhoDeCompra>()
			.ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.Quantidade));

		CreateMap<RequestLoginUsuarioJson, Usuario>();

		CreateMap<ItemCarrinhoDeCompra, ItemPedido>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.PedidoId, opt => opt.Ignore())
			.ForMember(dest => dest.Pedido, opt => opt.Ignore())
			.ForMember(dest => dest.Produto, opt => opt.Ignore())
			.ForMember(dest => dest.ProdutoId, opt => opt.MapFrom(src => src.ProdutoId))
			.ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.Quantidade))
			.ForMember(dest => dest.PrecoUnitario, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Valor : 0));
	}

	private void DomainToResponse()
	{
		CreateMap<Usuario, ResponsePerfilUsuarioJson>();

		CreateMap<Endereco, ResponseEnderecoJson>();

		CreateMap<Produto, ResponseProdutoJson>()
			.ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria.ToString()))
			.ForMember(dest => dest.QuantidadeEstoque, opt => opt.MapFrom(src => src.QuantidadeEstoque));

		CreateMap<ItemCarrinhoDeCompra, ResponseItemCarrinhoJson>()
			.ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.CalcularSubTotal()));

		CreateMap<CarrinhoDeCompras, ResponseCarrinhoDeComprasJson>()
			.ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.ItensCarrinho))
			.ForMember(dest => dest.Total, opt => opt.MapFrom(src =>
				src.ItensCarrinho != null ? src.ItensCarrinho.Sum(i => i.CalcularSubTotal()) : 0));

		CreateMap<Pedido, ResponsePedidoJson>();

		CreateMap<ItemPedido, ResponseItemPedidoJson>()
			.ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : string.Empty))
			.ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.Quantidade * src.PrecoUnitario));

		CreateMap<Pagamento, ResponsePagamentoJson>();
	}
}