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

		modelBuilder.Entity<Pedido>(pedido =>
		{
			pedido.HasKey(p => p.Id);
			pedido.Property(p => p.ValorTotal).HasColumnType("decimal(10,2)");
			pedido.HasOne(p => p.Usuario).WithMany(u => u.Pedidos).HasForeignKey(p => p.UsuarioId).OnDelete(DeleteBehavior.Restrict);
			pedido.HasOne(p => p.Endereco).WithMany().HasForeignKey(p => p.EnderecoId).OnDelete(DeleteBehavior.Restrict);
			pedido.HasMany(p => p.Itens).WithOne(ip => ip.Pedido).HasForeignKey(ip => ip.PedidoId).OnDelete(DeleteBehavior.Cascade);
			pedido.HasOne(p => p.Pagamento).WithOne(pag => pag.Pedido).HasForeignKey<Pagamento>(pag => pag.PedidoId);
		});

		modelBuilder.Entity<ItemPedido>(item =>
		{
			item.HasKey(i => i.Id);
			item.Property(i => i.PrecoUnitario).HasColumnType("decimal(10,2)");
			item.HasOne(i => i.Produto).WithMany().HasForeignKey(i => i.ProdutoId).OnDelete(DeleteBehavior.Restrict);
		});

		modelBuilder.Entity<Pagamento>(pagamento =>
		{
			pagamento.HasKey(p => p.Id);
			pagamento.Property(p => p.Valor).HasColumnType("decimal(10,2)");
		});

		modelBuilder.Entity<Produto>()
		.HasOne(p => p.Usuario)
		.WithMany()
		.HasForeignKey(p => p.UsuarioId)
		.OnDelete(DeleteBehavior.Restrict);
	}
}