using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_PEDIDO, "Criar tabelas para Pedido e ItemPedido")]
public class Versao0000007 : VersaoBase
{
	public override void Up()
	{
		CreateTable("Pedido")
			.WithColumn("UsuarioId").AsInt64().NotNullable().ForeignKey("FK_Pedido_Usuario", "Usuario", "Id")
			.WithColumn("EnderecoId").AsInt64().NotNullable().ForeignKey("FK_Pedido_Endereco", "Endereco", "Id")
			.WithColumn("Status").AsInt32().NotNullable()
			.WithColumn("DataPedido").AsCustom("timestamp with time zone").NotNullable()
			.WithColumn("ValorTotal").AsDecimal(10, 2).NotNullable();

		CreateTable("ItemPedido")
			.WithColumn("PedidoId").AsInt64().NotNullable().ForeignKey("Pedido", "Id")
			.WithColumn("ProdutoId").AsInt64().NotNullable().ForeignKey("Produto", "Id")
			.WithColumn("PrecoUnitario").AsDecimal().NotNullable()
			.WithColumn("Quantidade").AsInt32().NotNullable();
	}
}