using Aplicacao.Servicos;
using Aplicacao.UseCase.Carrinho.Atualizar;
using Aplicacao.UseCase.Carrinho.Buscar;
using Aplicacao.UseCase.Carrinho.Criar;
using Aplicacao.UseCase.Carrinho.Limpar;
using Aplicacao.UseCase.Carrinho.LimparCarrinho;
using Aplicacao.UseCase.Carrinho.RemoverItem;
using Aplicacao.UseCase.Dashboard.NovosPedidosHoje;
using Aplicacao.UseCase.Dashboard.ObterUltimosPedidos;
using Aplicacao.UseCase.Dashboard.ProdutosEmEstoque;
using Aplicacao.UseCase.Dashboard.ProdutosMaisVendidos;
using Aplicacao.UseCase.Dashboard.VendasHoje;
using Aplicacao.UseCase.Dashboard.VendasMensais;
using Aplicacao.UseCase.Pagamento.Criar;
using Aplicacao.UseCase.Pagamento.CriarMetodoPagamento;
using Aplicacao.UseCase.Pagamento.Excluir;
using Aplicacao.UseCase.Pagamento.Listar;
using Aplicacao.UseCase.Pagamento.ListarMetodoPagamento;
using Aplicacao.UseCase.Pedido.BuscarPorUsuario;
using Aplicacao.UseCase.Pedido.Criar;
using Aplicacao.UseCase.Pedido.ObterPedido;
using Aplicacao.UseCase.UseEndereco.Atualizar;
using Aplicacao.UseCase.UseEndereco.Buscar;
using Aplicacao.UseCase.UseEndereco.Criar;
using Aplicacao.UseCase.UseEndereco.Excluir;
using Aplicacao.UseCase.UseProduto.Atualizar;
using Aplicacao.UseCase.UseProduto.Criar;
using Aplicacao.UseCase.UseProduto.Desconto;
using Aplicacao.UseCase.UseProduto.Excluir;
using Aplicacao.UseCase.UseProduto.GetById;
using Aplicacao.UseCase.UseProduto.GetByVendedor;
using Aplicacao.UseCase.UseProduto.Imagem;
using Aplicacao.UseCase.UseProduto.ObetnhaProdutoPorCategoria;
using Aplicacao.UseCase.UseProduto.ObtenhaTodosProdutos;
using Aplicacao.UseCase.UseUsuario.AlterarSenha;
using Aplicacao.UseCase.UseUsuario.Atualizar;
using Aplicacao.UseCase.UseUsuario.Buscar;
using Aplicacao.UseCase.UseUsuario.Criar;
using Aplicacao.UseCase.UseUsuario.Excluir;
using Aplicacao.UseCase.UseUsuario.Login;
using Microsoft.Extensions.DependencyInjection;

namespace Aplicacao;

public static class InjecaoDeDependenciaExtensao
{
	public static void AdicioneAplicacao(this IServiceCollection services)
	{
		AddAutoMapper(services);
		AdicioneUseCase(services);
	}

	private static void AddAutoMapper(IServiceCollection services)
	{
		services.AddScoped(opt => new AutoMapper.MapperConfiguration(options =>
		{
			options.AddProfile(new AutoMapping());
		}).CreateMapper());
	}

	private static void AdicioneUseCase(IServiceCollection services)
	{
		services.AddScoped<IUsuarioUseCase, UsuarioUseCase>();
		services.AddScoped<IObterPerfilUsuarioUseCase, ObterPerfilUsuarioUseCase>();
		services.AddScoped<ILoginUseCase, LoginUseCase>();
		services.AddScoped<IAtualizarUsuarioUseCase, AtualizarUsuarioUseCase>();
		services.AddScoped<IExcluirUsuarioUseCase, ExcluirUsuarioUseCase>();
		services.AddScoped<IAlterarSenhaUsuarioUseCase, AlterarSenhaUsuarioUseCase>();
		services.AddScoped<IEnderecoUseCase, EnderecoUseCase>();
		services.AddScoped<IBuscarEnderecosUseCase, BuscarEnderecosUseCase>();
		services.AddScoped<IAtualizeEnderecoUseCase, AtualizeEnderecoUseCase>();
		services.AddScoped<IExcluirEnderecoUseCase, ExcluirEnderecoUseCase>();
		services.AddScoped<IProdutoUseCase, ProdutoUseCase>();
		services.AddScoped<IGetProdutoById, GetProdutoById>();
		services.AddScoped<IGetProdutosPorVendedorUseCase, GetProdutosPorVendedorUseCase>();
		services.AddScoped<IObtenhaTodosProdutos, ObtenhaTodosProdutos>();
		services.AddScoped<IObtenhaProdutosPorCategoria, ObtenhaProdutosPorCategoriaUseCase>();
		services.AddScoped<IExcluirProdutoUseCase, ExcluirProdutoUseCase>();
		services.AddScoped<IAtualizeProdutoUseCase, AtualizeProdutoUseCase>();
		services.AddScoped<ICarrinhoUseCase, CarrinhoUseCase>();
		services.AddScoped<IAtualizeQtdItemCarrinhoUseCase, AtualizeQtdItemCarrinhoUseCase>();
		services.AddScoped<IObterCarrinhoUseCase, ObterCarrinhoUseCase>();
		services.AddScoped<ILimpeCarrinhoUseCase, LimpeCarrinhoUseCase>();
		services.AddScoped<IRemoveItemCarrinhoUseCase, RemoveItemCarrinhoUseCase>();
		services.AddScoped<ICriarPedidoUseCase, CriarPedidoUseCase>();
		services.AddScoped<IObterPedidoPorIdUseCase, ObterPedidoPorIdUseCase>();
		services.AddScoped<IObterPedidosPorUsuarioUseCase, ObterPedidosPorUsuarioUseCase>();
		services.AddScoped<IAddUpdateImageCoverUseCase, AddUpdateImageCoverUseCase>();
		services.AddScoped<IObterVendasHojeUseCase, ObterVendasHojeUseCase>();
		services.AddScoped<IObterNovosPedidosHojeUseCase, ObterNovosPedidosHojeUseCase>();
		services.AddScoped<IObterProdutosEstoqueUseCase, ObterProdutosEstoqueUseCase>();
		services.AddScoped<IObterProdutosMaisVendidosUseCase, ObterProdutosMaisVendidosUseCase>();
		services.AddScoped<IObterUltimosPedidosUseCase, ObterUltimosPedidosUseCase>();
		services.AddScoped<IObterVendasMensaisUseCase, ObterVendasMensaisUseCase>();
		services.AddScoped<ICriarMetodoPagamentoUseCase, CriarMetodoPagamentoUseCase>();
		services.AddScoped<IListarMetodosPagamentoUseCase, ListarMetodosPagamentoUseCase>();
		services.AddScoped<IExcluirMetodoPagamentoUseCase, ExcluirMetodoPagamentoUseCase>();
		services.AddScoped<IAplicarDescontoUseCase, AplicarDescontoUseCase>();
	}
}