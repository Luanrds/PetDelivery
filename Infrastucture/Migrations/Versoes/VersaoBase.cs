﻿using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace Infrastucture.Migrations.Versoes;

public abstract class VersaoBase : ForwardOnlyMigration
{
    protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string tabela)
    {
        return Create.Table(tabela)
            .WithColumn("Id").AsInt64().PrimaryKey().Identity();
    }
}
