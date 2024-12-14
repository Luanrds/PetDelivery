using Dapper;
using Dominio.Interfaces.Generics;
using Dominio.Interfaces.InterfaceProduct;
using Entidades.Entidades;
using Infrastucture.Configuracao;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastucture.Repositorio.Repositorios;

public class RepositoryProduct : IProduct
{
    private readonly ContextBase _context;

    public RepositoryProduct(ContextBase context)
    {
        _context = context;
    }

    public async Task<bool> Add(Produto produto)
    {
        using var connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString);
        var query = "INSERT INTO Product (PRD_NOME, PRD_VALOR, PRD_ESTADO, PRD_DESCRICAO) VALUES (@Nome, @Valor, @Disponivel, @Descricao)";
        var result = await connection.ExecuteAsync(query, new
        {
            produto.Nome,
            produto.Valor,
            produto.Disponivel,
            produto.Descricao
        });

        return result > 0;
    }

    public async Task<List<Produto>> List()
    {
        using var connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString);
        var query = "SELECT PRD_ID, PRD_NOME, PRD_VALOR, PRD_ESTADO, PRD_DESCRICAO FROM Product";
        var produtos = await connection.QueryAsync<Produto>(query);
        return produtos.AsList();
    }

    public async Task<bool> Update(Produto produto)
    {
        using var connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString);
        var query = "UPDATE Product SET PRD_NOME = @Nome, PRD_VALOR = @Valor, PRD_ESTADO = @Disponivel, PRD_DESCRICAO = @Descricao WHERE PRD_ID = @Id";
        var result = await connection.ExecuteAsync(query, new
        {
            produto.Nome,
            produto.Valor,
            produto.Disponivel,
            produto.Descricao,
            produto.Id
        });

        return result > 0;
    }

    public async Task<Produto> GetEntityById(int id)
    {
        using var connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString);
        var query = "SELECT PRD_ID, PRD_NOME, PRD_VALOR, PRD_ESTADO, PRD_DESCRICAO FROM Product WHERE PRD_ID = @Id";
        var produto = await connection.QuerySingleOrDefaultAsync<Produto>(query, new { Id = id });
        return produto;
    }

    public async Task Delete(Produto produto)
    {
        using var connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString);
        var query = "DELETE FROM Product WHERE PRD_ID = @Id";
        var result = await connection.ExecuteAsync(query, new { produto.Id });

        if (result <= 0)
        {
            throw new Exception("Erro ao excluir produto.");
        }
    }

	Task<bool> IGenerics<Produto>.Delete(Produto Objeto)
	{
		throw new NotImplementedException();
	}
}
