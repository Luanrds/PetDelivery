using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_PAGAMENTO, "Criar tabela para PagamentoRepository")]
public class Versao0000007 : VersaoBase
{
	public override void Up()
	{
		CreateTable("Pagamento")
			.WithColumn("PedidoId").AsInt64().NotNullable().ForeignKey("FK_Pagamento_Pedido", "Pedido", "Id")
			.WithColumn("MetodoPagamento").AsInt32().NotNullable()
			.WithColumn("StatusPagamento").AsInt32().NotNullable()
			.WithColumn("Valor").AsDecimal(10, 2).NotNullable()
			.WithColumn("DataPagamento").AsDateTime().NotNullable();
	}
}
