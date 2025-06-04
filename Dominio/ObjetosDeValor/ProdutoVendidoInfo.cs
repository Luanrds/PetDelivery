using Dominio.Enums;

namespace Dominio.ObjetosDeValor;
public class ProdutoVendidoInfo
{
	public long ProdutoId { get; init; }
	public string NomeProduto { get; init; } = string.Empty;
	public CategoriaProduto Categoria { get; init; }
	public int QuantidadeVendas { get; init; }

	public ProdutoVendidoInfo(long produtoId, string nomeProduto, CategoriaProduto categoria, int quantidadeVendas)
	{
		ProdutoId = produtoId;
		NomeProduto = nomeProduto;
		Categoria = categoria;
		QuantidadeVendas = quantidadeVendas;
	}
}