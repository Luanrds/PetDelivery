using Entidades.Notificacoes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Entidades
{

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
}