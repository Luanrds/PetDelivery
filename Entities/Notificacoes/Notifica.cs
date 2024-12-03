using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Notificacoes;
public class Notifica
{
	[NotMapped]
	public string NomePropiedade { get; set; }

	[NotMapped]
	public string Mensagem { get; set; }

	public Notifica()
	{
		NomePropiedade = string.Empty;
		Mensagem = string.Empty;
	}
}
