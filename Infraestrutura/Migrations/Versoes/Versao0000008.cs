using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_PAGAMENTO_FK, "Adicionar chave estrangeira em Pagamento para Pedido")]
public class Versao0000008 : VersaoBase
{
	public override void Up()
	{
		Create.ForeignKey("FK_Pagamento_PedidoId_Pedido_Id")
			.FromTable("Pagamento").ForeignColumn("PedidoId")
			.ToTable("Pedido").PrimaryColumn("Id");
	}
}