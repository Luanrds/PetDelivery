using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_PAGAMENTO, "Criar tabela para PagamentoRepository")]
public class Versao0000005 : VersaoBase
{
	public override void Up()
	{
		CreateTable("Pagamento")
			.WithColumn("PedidoId").AsInt64().NotNullable()
			.WithColumn("MetodoPagamento").AsInt32().NotNullable()
			.WithColumn("StatusPagamento").AsInt32().NotNullable()
			.WithColumn("Valor").AsDecimal().NotNullable()
			.WithColumn("DataPagamento").AsDateTime().NotNullable();
	}
}