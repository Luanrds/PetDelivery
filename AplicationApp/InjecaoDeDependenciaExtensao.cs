using Aplicacao.Servicos;
using Aplicacao.UseCase.Carrinho;
using Aplicacao.UseCase.Carrinho.Atualizar;
using Aplicacao.UseCase.UseProduto.Atualizar;
using Aplicacao.UseCase.UseProduto.Criar;
using Aplicacao.UseCase.UseProduto.Excluir;
using Aplicacao.UseCase.UseProduto.GetById;
using Aplicacao.UseCase.UseProduto.ObtenhaTodosProdutos;
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
        services.AddScoped<IProdutoUseCase, ProdutoUseCase>();
        services.AddScoped<IGetProdutoById, GetProdutoById>();
        services.AddScoped<IObtenhaTodosProdutos, ObtenhaTodosProdutos>();
        services.AddScoped<IExcluirProdutoUseCase, ExcluirProdutoUseCase>();
        services.AddScoped<IAtualizeProdutoUseCase, AtualizeProdutoUseCase>();
        services.AddScoped<ICarrinhoUseCase, CarrinhoUseCase>();
        services.AddScoped<IAtualizeQtdItemCarrinhoUseCase, AtualizeQtdItemCarrinhoUseCase>();
    }
}