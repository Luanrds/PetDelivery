using Microsoft.Extensions.Configuration;

namespace Infraestrutura.Extensoes;

public static class ConfiguracoesDeExtensoes
{
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("Connection")!;
    }
}
