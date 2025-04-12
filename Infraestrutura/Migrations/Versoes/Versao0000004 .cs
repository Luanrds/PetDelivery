using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_CARRINHO, "Criar tabelas para CarrinhoDeCompras e ItemCarrinhoDeCompra")]
public class Versao0000004 : VersaoBase
{
	public override void Up()
	{
		CreateTable("CarrinhoDeCompras")
			.WithColumn("UsuarioId").AsInt64().NotNullable().ForeignKey("Usuario", "Id");

		CreateTable("ItemCarrinhoDeCompra")
			.WithColumn("CarrinhoId").AsInt64().NotNullable().ForeignKey("CarrinhoDeCompras", "Id")
			.WithColumn("ProdutoId").AsInt64().NotNullable().ForeignKey("Produto", "Id")
			.WithColumn("Quantidade").AsInt32().NotNullable()
			.WithColumn("PrecoUnitario").AsDecimal().NotNullable();
	}
}