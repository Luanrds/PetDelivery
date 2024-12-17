using Dapper;
using Npgsql;

namespace Infrastucture.Migrations;

public static class BancoDeDadosMigration
{
    public static void Migrate(string connectionString)
    {
        GaranteCriacaoDoBancoDeDados(connectionString);
    }

    private static void GaranteCriacaoDoBancoDeDados(string connectionString)
    {
        NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString);

        string? NomeDoBancoDeDados = connectionStringBuilder.Database;

        connectionStringBuilder.Database = "postgres";

        using var dbConnection = new NpgsqlConnection(connectionStringBuilder.ConnectionString);

        IEnumerable<dynamic> records = dbConnection.Query(
            "SELECT datname FROM pg_database WHERE datname = @nome;", 
            new { nome = NomeDoBancoDeDados });

        if (!records.Any())
        {
            var query = $"CREATE DATABASE \"{NomeDoBancoDeDados}\";";
            dbConnection.Execute(query);
        }
    }
}
