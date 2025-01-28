using FluentMigrator;

namespace Infrastucture.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_USUARIO, "Criar tabela para salvar as informações do usuário ")]
public class Versao0000003 : VersaoBase
{
	public override void Up()
	{
		CreateTable("Usuario")
			.WithColumn("Nome").AsString(255).NotNullable()
			.WithColumn("Email").AsString(255).NotNullable()
			.WithColumn("Senha").AsString(2000).NotNullable();
	}
}
