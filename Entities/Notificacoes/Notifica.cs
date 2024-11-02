using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Notificacoes;
public class Notifica
{
	[NotMapped]
	public string NomePropiedade { get; set; }

	[NotMapped]
	public string Mensagem { get; set; }

	[NotMapped]
	public List<Notifica> Notificacoes { get; set; }

	public Notifica()
	{
		Notificacoes = [];
	}

	public bool ValidarPropriedadeString(string valor, string nomePropriedade)
	{
		if (string.IsNullOrWhiteSpace(valor) || string.IsNullOrWhiteSpace(nomePropriedade))
		{
			Notificacoes.Add(new Notifica
			{
				Mensagem = "Campo Obrigatório",
				NomePropiedade = nomePropriedade
			});
			return false;
		}
		return true;
	}

	public bool ValidarPropriedadeInt(int valor, string nomePropriedade)
	{
		if (valor < 1 || string.IsNullOrWhiteSpace(nomePropriedade))
		{
			Notificacoes.Add(new Notifica
			{
				Mensagem = "Valor deve ser maior que 0",
				NomePropiedade = nomePropriedade
			});
			return false;
		}
		return true;
	}

	public bool ValidarPropriedadeDecimal(decimal valor, string nomePropriedade)
	{
		if (valor < 1 || string.IsNullOrWhiteSpace(nomePropriedade))
		{
			Notificacoes.Add(new Notifica
			{
				Mensagem = "Valor deve ser maior que 0",
				NomePropiedade = nomePropriedade
			});
			return false;
		}
		return true;
	}
}
