using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Configuracao;

public class PetDeliveryDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Produto> Produto { get; set; }
    public DbSet<CarrinhoDeCompras> CarrinhoDeCompras { get; set; }
	public DbSet<ItemCarrinhoDeCompra> ItemCarrinhoDeCompra { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetDeliveryDbContext).Assembly);

		modelBuilder.Entity<ItemCarrinhoDeCompra>()
			.HasOne(i => i.Produto)
			.WithMany()
			.HasForeignKey(i => i.ProdutoId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}

