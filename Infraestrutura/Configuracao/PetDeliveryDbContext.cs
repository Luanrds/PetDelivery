using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
	public DbSet<MetodoPagamentoUsuario> MetodoPagamentoUsuario { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetDeliveryDbContext).Assembly);

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

		modelBuilder.Entity<Produto>(builder =>
		{
			builder.HasOne(p => p.Usuario)
				   .WithMany()
				   .HasForeignKey(p => p.UsuarioId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.Property(p => p.ImagensIdentificadores)
				.HasColumnType("jsonb")
				.HasConversion(
					v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
					v => string.IsNullOrEmpty(v)
						 ? new List<string>()
						 : JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new List<string>()
				)
				.Metadata.SetValueComparer(new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
					(c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.SequenceEqual(c2)),
					c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
					c => c.ToList()));
		});
	}
}