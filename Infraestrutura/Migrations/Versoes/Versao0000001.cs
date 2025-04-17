using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_PRODUTO, "Criar tabela para salvar as informações do Produto ")]
public class Versao0000001 : VersaoBase
{
    public override void Up()
    {
		CreateTable("Produto")
            .WithColumn("Nome").AsString(255).NotNullable()
            .WithColumn("Descricao").AsString(1000).Nullable()
            .WithColumn("Valor").AsDecimal().NotNullable()
			.WithColumn("Categoria").AsInt32().NotNullable()
            .WithColumn("QuantidadeEstoque").AsInt32().NotNullable();
	}
}
