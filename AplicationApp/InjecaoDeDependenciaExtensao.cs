using Aplicacao.UseCase.UseProduto;
using Aplicacao.Servicos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Sqids;

namespace Aplicacao;

public static class InjecaoDeDependenciaExtensao
{
    public static void AdicioneAplicacao(this IServiceCollection services, IConfiguration configuration)
    {
        AddAutoMapper(services);
		AddIdEncoder(services, configuration);
		AdicioneUseCase(services);
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
		services.AddScoped(opt => new AutoMapper.MapperConfiguration(options =>
        {
			var sqids = opt.GetService<SqidsEncoder<long>>()!;

			options.AddProfile(new AutoMapping(sqids));
        }).CreateMapper());
    }

	private static void AddIdEncoder(IServiceCollection services, IConfiguration configuration)
	{
		var sqids = new SqidsEncoder<long>(new()
		{
			MinLength = 3,
			Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
		});

		services.AddSingleton(sqids);
	}

	private static void AdicioneUseCase(IServiceCollection services)
    {
        services.AddScoped<IProdutoUseCase, ProdutoUseCase>();
    }
}