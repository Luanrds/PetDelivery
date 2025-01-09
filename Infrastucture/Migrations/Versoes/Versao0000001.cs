using FluentMigrator;

namespace Infrastucture.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_PRODUTO, "Criar tabela para salvar as informações do Produto ")]
public class Versao0000001 : VersaoBase
{
    public override void Up()
    {
        Create.Table("Produto")
            .WithColumn("Nome").AsString(255).NotNullable()
            .WithColumn("Valor").AsDecimal().NotNullable()
            .WithColumn("Disponivel").AsBoolean().NotNullable()
            .WithColumn("Descricao").AsString(1000).Nullable();
    }
}
