﻿using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace Infraestrutura.Configuracao;

public class PetDeliveryDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Produto> Produto { get; set; }
    public DbSet<CarrinhoDeCompras> CarrinhoDeCompras { get; set; }
	public DbSet<ItemCarrinhoDeCompra> ItemCarrinhoDeCompra { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PetDeliveryDbContext).Assembly);
	}
}
