using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.UNACENT, "Habilita a extensão unaccent no PostgreSQL")]
public class Versao000009 : VersaoBase
{
	public override void Up()
	{
		Execute.Sql("CREATE EXTENSION IF NOT EXISTS unaccent;");
	}
}