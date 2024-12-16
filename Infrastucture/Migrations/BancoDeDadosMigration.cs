using Dapper;
using Npgsql;

namespace Infrastucture.Migrations;

public static class BancoDeDadosMigration
{
    public static void Migrate(string connectionString)
    {
        GarantiaBancoDadosCriado(connectionString);
    }

    private static void GarantiaBancoDadosCriado(string connectionString)
    {
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);

        var schemaName = connectionStringBuilder.Database;

        connectionStringBuilder.Remove("DataBase");

        DynamicParameters parameters = new DynamicParameters();
        parameters.Add("nome", schemaName);

        using NpgsqlConnection dbConnection = new(connectionStringBuilder.ConnectionString);

        var records = dbConnection.Query("SELECT schema_name FROM information_schema.schemata WHERE schema_name = @nome;", parameters);

        if (!records.Any())
        {
            dbConnection.Execute("CREATE SCHEMA @nome;", parameters);
        }
    }

}
