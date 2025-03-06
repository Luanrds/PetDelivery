using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_USUARIO, "Criar tabelas para usuarios")]
public class Versao0000002 : VersaoBase
{
	public override void Up()
	{
		CreateTable("Usuario")
			.WithColumn("Nome").AsString(255).NotNullable()
			.WithColumn("Email").AsString(255).NotNullable().Unique()
			.WithColumn("Senha").AsString(2000).NotNullable()
			.WithColumn("DataNascimento").AsDateTime().Nullable();
	}
}