using Microsoft.Extensions.Configuration;

namespace Infrastucture.Extensoes;

public static class ConfiguracoesDeExtensoes
{
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("Connection")!;
    }
}
