using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_METODOPAGAMENTO, "Criar tabela para Metodo de pagamento")]
public class Versao0000006 : VersaoBase
{
	public override void Up()
	{
		CreateTable("MetodoPagamentoUsuario")
			.WithColumn("UsuarioId").AsInt64().NotNullable().ForeignKey("FK_MetodoPagamento_Usuario", "Usuario", "Id")
			.WithColumn("NomeTitular").AsString(100).NotNullable()
			.WithColumn("NumeroCartao").AsString(20).NotNullable()
			.WithColumn("DataValidade").AsString(5).NotNullable()
			.WithColumn("Tipo").AsInt32().NotNullable();
	}
}