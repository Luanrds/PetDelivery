using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Configuracao;

public class PetDeliveryDbContext(DbContextOptions<PetDeliveryDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Produto> Produto { get; set; }
    public DbSet<CarrinhoDeCompras> CarrinhoDeCompras { get; set; }
    public DbSet<ItemCarrinhoDeCompra> ItemCarrinhoDeCompra { get; set; }
    public DbSet<Endereco> Endereco { get; set; }
    public DbSet<Pedido> Pedido { get; set; }
    public DbSet<Pagamento> Pagamento { get; set; }
    public DbSet<ItemPedido> ItemPedido { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetDeliveryDbContext).Assembly);

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Pagamento)
            .WithOne(pg => pg.Pedido)
            .HasForeignKey<Pagamento>(pg => pg.PedidoId);

        modelBuilder.Entity<ItemCarrinhoDeCompra>()
            .HasOne<CarrinhoDeCompras>()
            .WithMany(c => c.ItensCarrinho)
            .HasForeignKey(i => i.CarrinhoId);
    }
}