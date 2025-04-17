using Dominio.Enums;

namespace Dominio.Entidades;

public class Produto : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;   
    public decimal Valor { get; set; }
	public CategoriaProduto Categoria { get; set; } 
    public int QuantidadeEstoque { get; set; }
}