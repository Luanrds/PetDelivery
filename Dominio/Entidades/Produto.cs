using Dominio.Enums;

namespace Dominio.Entidades;

public class Produto : EntidadeBase
{
    public string Nome { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public bool Disponivel { get; set; }
    public string Descricao { get; set; } = string.Empty;   
    public CategoriaProduto CategoriaProduto { get; set; } 
}