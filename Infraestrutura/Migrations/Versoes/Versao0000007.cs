using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_PEDIDO, "Criar tabelas para Pedido e ItemPedido")]
public class Versao0000007 : VersaoBase
{
	public override void Up()
	{
		CreateTable("Pedido")
			.WithColumn("ClienteId").AsInt64().NotNullable().ForeignKey("Usuario", "Id")
			.WithColumn("EnderecoEntregaId").AsInt64().NotNullable().ForeignKey("Endereco", "Id")
			.WithColumn("PagamentoId").AsInt64().NotNullable().ForeignKey("Pagamento", "Id")
			.WithColumn("Status").AsString(50).NotNullable()
			.WithColumn("DataPedido").AsDateTime().NotNullable();

		CreateTable("ItemPedido")
			.WithColumn("PedidoId").AsInt64().NotNullable().ForeignKey("Pedido", "Id")
			.WithColumn("ProdutoId").AsInt64().NotNullable().ForeignKey("Produto", "Id")
			.WithColumn("PrecoUnitario").AsDecimal().NotNullable()
			.WithColumn("Quantidade").AsInt32().NotNullable();
	}
}