using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades;

[Table("Product")]
public class Produto : EntidadeBase
{
    [Display(Name = "Nome")]
    public string Nome { get; set; } = string.Empty;

    [Display(Name = "Valor")]
    public decimal Valor { get; set; }

    [Display(Name = "Disponível")]
    public bool Disponivel { get; set; }

    [Display(Name = "Descrição")]
    [MaxLength(500)]
    public string Descricao { get; set; } = string.Empty;   
}