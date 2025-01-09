using Aplicacao.UseCase.UseProduto;
using Aplicacao.Servicos;
using Microsoft.Extensions.DependencyInjection;
using Aplicacao.UseCase.UseProduto.GetById;

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
        services.AddScoped<IProdutoUseCase, ProdutoUseCase>();
        services.AddScoped<IGetProdutoById, GetProdutoById>();
    }
}