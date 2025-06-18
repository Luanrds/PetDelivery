using AutoMapper;
using Dominio.Entidades;
using Dominio.ObjetosDeValor;
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
			.ForMember(dest => dest.PrecoUnitarioPago, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Valor : 0));

		CreateMap<RequestCartaoCreditoJson, MetodoPagamentoUsuario>();
	}

	private void DomainToResponse()
	{
		CreateMap<Usuario, ResponsePerfilUsuarioJson>();

		CreateMap<Endereco, ResponseEnderecoJson>();

		CreateMap<Produto, ResponseProdutoJson>()
			.ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => (int)src.Categoria))
			.ForMember(dest => dest.QuantidadeEstoque, opt => opt.MapFrom(src => src.QuantidadeEstoque))
			.ForMember(dest => dest.ValorOriginal, opt => opt.MapFrom(src => src.Valor))
			.ForMember(dest => dest.ValorComDesconto, opt => opt.MapFrom(src => src.ObterPrecoFinal()))
			.ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.ValorDesconto))
			.ForMember(dest => dest.TipoDesconto, opt => opt.MapFrom(src => (int?)src.TipoDesconto))
			.ForMember(dest => dest.ImagemUrl, opt => opt.MapFrom(src =>
				(src.ImagensIdentificadores != null && src.ImagensIdentificadores.Count != 0)
				? src.ImagensIdentificadores.First()
				: null))
			 .ForMember(dest => dest.ImagensUrl, opt => opt.Ignore());


		CreateMap<ItemCarrinhoDeCompra, ResponseItemCarrinhoJson>()
			.ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.CalcularSubTotal()))
			.ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Produto.Nome))
			.ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Produto.DescricaoResumida))
			.ForMember(dest => dest.ImagemUrl, opt => opt.Ignore())
			.ForMember(dest => dest.PrecoUnitarioOriginal, opt => opt.MapFrom(src => src.Produto.Valor))
			.ForMember(dest => dest.PrecoUnitarioComDesconto, opt => opt.MapFrom(src =>
				src.Produto.ValorDesconto.HasValue ? src.Produto.ObterPrecoFinal() : (decimal?)null))
			.ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.Produto.ValorDesconto))
			.ForMember(dest => dest.TipoDesconto, opt => opt.MapFrom(src => (int?)src.Produto.TipoDesconto))
			.ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.CalcularSubTotal()));

		CreateMap<CarrinhoDeCompras, ResponseCarrinhoDeComprasJson>()
			.ForMember(dest => dest.Itens, opt => opt.MapFrom(src => src.ItensCarrinho))
			.ForMember(dest => dest.Total, opt => opt.MapFrom(src =>
				src.ItensCarrinho != null ? src.ItensCarrinho.Sum(i => i.CalcularSubTotal()) : 0));

		CreateMap<Pedido, ResponsePedidoJson>();

		CreateMap<ItemPedido, ResponseItemPedidoJson>()
			.ForMember(dest => dest.NomeProduto, opt => opt.MapFrom(src => src.Produto != null ? src.Produto.Nome : string.Empty))
			.ForMember(dest => dest.ImagemUrl, opt => opt.MapFrom(src =>
				(src.Produto.ImagensIdentificadores != null && src.Produto.ImagensIdentificadores.Count != 0)
				? src.Produto.ImagensIdentificadores.First()
				: null))
			.ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.Quantidade * src.PrecoUnitarioPago))
			.ForMember(dest => dest.PrecoUnitarioOriginal, opt => opt.MapFrom(src => src.PrecoUnitarioOriginal))
			.ForMember(dest => dest.PrecoUnitarioPago, opt => opt.MapFrom(src => src.PrecoUnitarioPago))
			.ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.ValorDesconto))
			.ForMember(dest => dest.TipoDesconto, opt => opt.MapFrom(src => (int?)src.TipoDesconto));

		CreateMap<Pagamento, ResponsePagamentoJson>();

		CreateMap<ProdutoVendidoInfo, ResponseProdutoMaisVendidoJson>()
			.ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.Categoria.ToString()));

		CreateMap<Pedido, ResponsePedidoCriadoJson>()
			.ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.StatusInicial, opt => opt.MapFrom(src => src.Status.ToString()));

		CreateMap<Pedido, ResponseUltimoPedidoJson>()
			.ForMember(dest => dest.PedidoId, opt => opt.MapFrom(src => $"#{src.Id}"))
		    .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nome : "Cliente Desconhecido"))
			.ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

		CreateMap<MetodoPagamentoUsuario, ResponseCartaoCreditoJson>()
			.ForMember(dest => dest.NumeroCartaoUltimosQuatro, opt => opt.MapFrom(src =>
				!string.IsNullOrEmpty(src.NumeroCartao) && src.NumeroCartao.Length > 4
				? src.NumeroCartao.Substring(src.NumeroCartao.Length - 4)
				: string.Empty));
	}
}