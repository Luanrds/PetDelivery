using Dominio.Enums;

namespace Dominio.Entidades;

public class Produto : EntidadeBase
{
	public long UsuarioId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
	public string DescricaoResumida { get; set; } = string.Empty;
	public decimal Valor { get; set; }
	public decimal? ValorDesconto { get; set; }
	public TipoDesconto? TipoDesconto { get; set; }
	public CategoriaProduto Categoria { get; set; } 
    public int QuantidadeEstoque { get; set; }
	public List<string> ImagensIdentificadores { get; set; } = [];
	public virtual Usuario Usuario { get; set; } = null!;

	public decimal ObterPrecoFinal()
	{
		if (!ValorDesconto.HasValue || !TipoDesconto.HasValue)
		{
			return Valor;
		}

		if (TipoDesconto.Value == Enums.TipoDesconto.ValorFixo)
		{
			decimal precoComDesconto = Valor - ValorDesconto.Value;
			return precoComDesconto > 0 ? precoComDesconto : 0;
		}

		if (TipoDesconto.Value == Enums.TipoDesconto.Porcentagem)
		{
			decimal desconto = Valor * (ValorDesconto.Value / 100);
			return Valor - desconto;
		}

		return Valor;
	}
}