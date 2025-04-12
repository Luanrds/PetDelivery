using FluentMigrator;
using System;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_ENDERECO, "Criar tabela para Endereco")] 
public class Versao0000003 : VersaoBase
{
	public override void Up()
	{
		CreateTable("Endereco")
			.WithColumn("UsuarioId").AsInt64().NotNullable().ForeignKey("Usuario", "Id")
			.WithColumn("Rua").AsString(255).NotNullable()
			.WithColumn("Numero").AsString(50).NotNullable()
			.WithColumn("Bairro").AsString(255).NotNullable()
			.WithColumn("Cidade").AsString(255).NotNullable()
			.WithColumn("Estado").AsString(50).NotNullable()
			.WithColumn("CEP").AsString(20).NotNullable();
	}
}
