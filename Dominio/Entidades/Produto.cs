using Dominio.Enums;

namespace Dominio.Entidades;

public class Produto : EntidadeBase
{
	public long UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;   
    public decimal Valor { get; set; }
	public CategoriaProduto Categoria { get; set; } 
    public int QuantidadeEstoque { get; set; }
    public string? ImagemIdentificador { get; set; }
	public virtual Usuario Usuario { get; set; } = null!;
}