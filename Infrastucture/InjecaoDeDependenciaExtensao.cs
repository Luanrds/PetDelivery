using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Infrastucture.Configuracao;
using Infrastucture.Repositorio.Repositorios;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastucture;

public static class InjecaoDeDependenciaExtensao
{
    public static void AdicioneInfraestrutura(this IServiceCollection services)
    {
        AddDbContext_Npga(services);
		AdicioneRepositorios(services);
    }

    private static void AddDbContext_Npga(IServiceCollection services)
    {
        var connectionString = "Host=localhost;Port=5432;Database=PetDelivery;Username=postgres;Password=0001";

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
