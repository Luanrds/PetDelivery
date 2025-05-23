using FluentMigrator;

namespace Infraestrutura.Migrations.Versoes;

[Migration(VersoesDeBancoDeDados.TABLE_PRODUTO, "Criar tabela para salvar as informações do Produto ")]
public class Versao0000002 : VersaoBase
{
    public override void Up()
    {
        CreateTable("Produto")
            .WithColumn("UsuarioId").AsInt64().NotNullable().ForeignKey("FK_Produto_Usuario", "Usuario", "Id")
            .WithColumn("Nome").AsString(255).NotNullable()
            .WithColumn("Descricao").AsString(1000).Nullable()
            .WithColumn("Valor").AsDecimal().NotNullable()
            .WithColumn("Categoria").AsInt32().NotNullable()
            .WithColumn("QuantidadeEstoque").AsInt32().NotNullable()
            .WithColumn("ImagemIdentificador").AsString().Nullable();
	}
}
