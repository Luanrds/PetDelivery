using System.ComponentModel.DataAnnotations;

namespace Dominio.Entidades;
public class EntidadeBase
{
    [Display(Name = "Código")]
    public int Id { get; set; }
}
