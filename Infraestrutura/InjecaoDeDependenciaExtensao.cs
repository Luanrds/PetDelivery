using Dominio.Repositorios;
using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios.Endereco;
using Dominio.Repositorios.Pagamento;
using Dominio.Repositorios.Pedido;
using Dominio.Repositorios.Produto;
using Dominio.Repositorios.Usuario;
using FluentMigrator.Runner;
using Infraestrutura.Configuracao;
using Infraestrutura.Extensoes;
using Infraestrutura.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Infraestrutura;

public static class InjecaoDeDependenciaExtensao
{
    public static void AdicioneInfraestrutura(this IServiceCollection services, IConfiguration configuration)
    {
        AdicioneDbContext_Npga(services, configuration);

        AdicioneFluentMigrator_Npga(services, configuration);

        AdicioneRepositorios(services);
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
}
