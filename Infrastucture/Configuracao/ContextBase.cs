using Dapper;
using Entidades.Entidades;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Infrastucture.Configuracao;

public class ContextBase : DbContext
{
    public ContextBase(DbContextOptions<ContextBase> options) : base(options) { }

    public DbSet<Produto> Produtos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=my_host;Database=my_db;Username=my_user;Password=my_password");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>().ToTable("Product");
    }
}
