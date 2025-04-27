using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_USUARIO, "Criar tabelas para usuarios")]
public class Versao0000002 : VersaoBase
{
	public override void Up()
	{
		CreateTable("Usuario")
			.WithColumn("Ativo").AsBoolean().NotNullable()
			.WithColumn("DataCadastro").AsCustom("timestamp with time zone").NotNullable()
			.WithColumn("Nome").AsString(255).NotNullable()
			.WithColumn("Email").AsString(255).NotNullable().Unique()
			.WithColumn("Senha").AsString(2000).NotNullable()
			.WithColumn("IdentificadorDoUsuario").AsGuid().NotNullable();
			//.WithColumn("EhVendedor").AsBoolean().NotNullable().WithDefaultValue(false);	
	}
}