using Entidades.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Configuracao;

public class ContextBase : IdentityDbContext<ApplicationUser>
{

    public ContextBase(DbContextOptions<ContextBase> options) : base(options)
    {
    }

    public DbSet<Produto> Produtoo { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(GetStringConnectionConfig());
            base.OnConfiguring(optionsBuilder);
        }

    }

    private string GetStringConnectionConfig()
    {
        string strCon = "Host=localhost;Port=5432;Database=DDD_ECOMMERCE;Username=seu_usuario;Password=sua_senha;";
        return strCon;
    }

}
