using Dapper;
using Dominio.Entidades;
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
            optionsBuilder.UseNpgsql("Host=5432;Database=petDelivey;Username=postgres;Password=0001");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Produto>().ToTable("Product");
    }
}
