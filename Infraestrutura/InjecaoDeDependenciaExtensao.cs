using Azure.Storage.Blobs;
using Dominio.Extensoes;
using Dominio.Repositorios;
using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios.Endereco;
using Dominio.Repositorios.Pagamento;
using Dominio.Repositorios.Pedido;
using Dominio.Repositorios.Produto;
using Dominio.Repositorios.Usuario;
using Dominio.Seguranca.Criptografia;
using Dominio.Seguranca.Tokens;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using FluentMigrator.Runner;
using Infraestrutura.Configuracao;
using Infraestrutura.Extensoes;
using Infraestrutura.Repositorio.Repositorios;
using Infraestrutura.Seguranca.Criptografia;
using Infraestrutura.Seguranca.Tokens.Access.Generator;
using Infraestrutura.Seguranca.Tokens.Access.Validador;
using Infraestrutura.Servicos.Storage;
using Infraestrutura.Servicos.UsuarioLogado;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infraestrutura;

public static class InjecaoDeDependenciaExtensaoRG
{
	public static void AdicioneInfraestrutura(this IServiceCollection services, IConfiguration configuration)
	{
		AddPasswordEncrpter(services);
		AddTokens(services, configuration);
		AddUsuarioLogado(services);
		AdicioneDbContext_Npga(services, configuration);
		AdicioneFluentMigrator_Npga(services, configuration);
		AdicioneRepositorios(services);
		AddAzureStorage(services, configuration);
	}

	private static void AdicioneDbContext_Npga(IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.ConnectionString();

		services.AddDbContext<PetDeliveryDbContext>(options => // <= Mudança aqui, renomeei 'dbContext' para 'options' para clareza
		{
			options.UseNpgsql(connectionString);

			// ----> ADICIONE ESTA LINHA <----
			options.LogTo(Console.WriteLine, LogLevel.Information);
			// -----------------------------

			// OPCIONAL: Para ver os valores dos parâmetros (CUIDADO EM PRODUÇÃO)
			// options.EnableSensitiveDataLogging();
		});
	}

	private static void AdicioneRepositorios(IServiceCollection services)
	{
		services.AddScoped<IUnitOfWork, UnitOfWork>();

		services.AddScoped<IUsuarioWriteOnly, UsuarioRepository>();
		services.AddScoped<IUsuarioReadOnly, UsuarioRepository>();
		services.AddScoped<IUsuarioUpdateOnly, UsuarioRepository>();
		services.AddScoped<IEnderecoWriteOnly, EnderecoRepository>();
		services.AddScoped<IEnderecoReadOnly, EnderecoRepository>();
		services.AddScoped<IProdutoWriteOnly, ProdutoRepository>();
		services.AddScoped<IProdutoReadOnly, ProdutoRepository>();
		services.AddScoped<IProdutoUpdateOnly, ProdutoRepository>();
		services.AddScoped<ICarrinhoReadOnly, CarrinhoRepository>();
		services.AddScoped<ICarrinhoWriteOnly, CarrinhoRepository>();
		services.AddScoped<IPedidoReadOnly, PedidoRepository>();
		services.AddScoped<IPedidoWriteOnly, PedidoRepository>();
		services.AddScoped<IPagamentoWriteOnly, PagamentoRepository>();
	}

	private static void AdicioneFluentMigrator_Npga(IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.ConnectionString();

		services.AddFluentMigratorCore().ConfigureRunner(options =>
		{
			options
			.AddPostgres()
			.WithGlobalConnectionString(connectionString)
			.ScanIn(Assembly.Load("Infraestrutura")).For.All();
		});
	}

	private static void AddPasswordEncrpter(IServiceCollection services)
	{
		services.AddScoped<ISenhaEncripter, BCryptNet>();
	}

	private static void AddTokens(IServiceCollection services, IConfiguration configuration)
	{
		var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
		var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

		services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
		services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));
	}

	private static void AddUsuarioLogado(IServiceCollection services) =>
		services.AddScoped<IUsuarioLogado, UsuarioLogado>();

	private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
	{
		var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

		if (connectionString.NotEmpty())
		{
			services.AddScoped<IBlobStorageService>(c => new AzureStorageService(new BlobServiceClient(connectionString)));
		}
	}
}
