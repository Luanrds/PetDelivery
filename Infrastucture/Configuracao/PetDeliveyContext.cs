using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Configuracao;

public class PetDeliveyContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Produto> Produto { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetDeliveyContext).Assembly);
    }
}
