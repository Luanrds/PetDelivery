using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Configuracao;

public class PetDeliveryDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Produto> Produto { get; set; }
    public DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetDeliveryDbContext).Assembly);
    }
}
