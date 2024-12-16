using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Infrastucture.Configuracao;
using Infrastucture.Extensoes;
using Infrastucture.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastucture;

public static class InjecaoDeDependenciaExtensao
{
    public static void AdicioneInfraestrutura(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext_Npga(services, configuration);
		AdicioneRepositorios(services);
    }

    private static void AddDbContext_Npga(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.ConnectionString();

        services.AddDbContext<PetDeliveyContext>(dbContext =>
        {
            dbContext.UseNpgsql(connectionString);
        });
    }

    private static void AdicioneRepositorios(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IProdutoWriteOnly, ProdutoRepository>();
        services.AddScoped<IProdutoReadOnly, ProdutoRepository>();
    }
}
