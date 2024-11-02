using Entidades.Notificacoes;
using System.ComponentModel.DataAnnotations;

namespace Entidades.Entidades;
public class Base : Notifica
{
    [Display(Name = "Código")]
    public int Id { get; set; }

    [Display(Name = "Nome")]
    public string Nome { get; set; }
}
