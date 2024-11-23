using Entidades.Notificacoes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Entidades;

[Table("Product")]
public class Produto : Notifica
{

    [Column("PRD_ID")]
    [Display(Name = "Código")]

    public int Id { get; set; }

    [Column("PRD_NOME")]
    [Display(Name = "Nome")]

    public string Nome { get; set; }

    [Column("PRD_VALOR")]
    [Display(Name = "Valor")]

    public decimal Valor { get; set; }

    [Column("PRD_ESTADO")]
    [Display(Name = "Estado")]

    public bool Estado { get; set; }

}