using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Carrinho;
using Dominio.Repositorios.Produto;
using Dominio.Repositorios.Usuario;
using FluentMigrator.Runner;
using Infrastucture.Configuracao;
using Infrastucture.Extensoes;
using Infrastucture.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastucture;

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

        services.AddDbContext<PetDeliveryDbContext>(dbContext =>
        {
            dbContext.UseNpgsql(connectionString);
        });
    }

    private static void AdicioneRepositorios(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IProdutoWriteOnly, ProdutoRepository>();
        services.AddScoped<IProdutoReadOnly, ProdutoRepository>();
        services.AddScoped<IProdutoUpdateOnly, ProdutoRepository>();
        services.AddScoped<IUsuarioReadOnlyRepository, UsuarioRepository>();
        services.AddScoped<IUsuarioWriteOnlyRepository, UsuarioRepository>();
        services.AddScoped<ICarrinhoReadOnly, CarrinhoRepository>();
        services.AddScoped<ICarrinhoWriteOnly, CarrinhoRepository>();

    }

    private static void AdicioneFluentMigrator_Npga(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddFluentMigratorCore().ConfigureRunner(options =>
        {
            options
            .AddPostgres()
            .WithGlobalConnectionString(connectionString)
            .ScanIn(Assembly.Load("Infrastucture")).For.All();
        });
    }
}
