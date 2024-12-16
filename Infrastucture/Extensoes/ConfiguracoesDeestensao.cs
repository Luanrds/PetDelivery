using Microsoft.Extensions.Configuration;

namespace Infrastucture.Extensoes;

public static class ConfiguracoesDeestensao
{
    public static string ConnectionString(this IConfiguration configuration)
    {
        return configuration.GetConnectionString("Connection")!;
    }
}
