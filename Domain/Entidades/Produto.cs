using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio.Entidades;

[Table("Product")]
public class Produto : EntidadeBase
{
    [Display(Name = "Nome")]
    public required string Nome { get; set; }

    [Display(Name = "Valor")]
    public decimal Valor { get; set; }

    [Display(Name = "Disponível")]
    public bool Disponivel { get; set; }

    [Display(Name = "Descrição")]
    [MaxLength(500)]
    public required string Descricao { get; set; }
}