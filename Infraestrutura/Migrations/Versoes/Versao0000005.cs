using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_PAGAMENTO, "Criar tabela para Pagamento")]
public class Versao0000005 : VersaoBase
{
	public override void Up()
	{
		CreateTable("Pagamento")
			.WithColumn("PedidoId").AsInt64().NotNullable()
			.WithColumn("Metodo").AsString(50).NotNullable()
			.WithColumn("Status").AsString(50).NotNullable()
			.WithColumn("Valor").AsDecimal().NotNullable()
			.WithColumn("DataPagamento").AsDateTime().NotNullable();
	}
}