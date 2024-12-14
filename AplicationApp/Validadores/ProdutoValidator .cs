﻿using Dominio.Entidades;
using FluentValidation;

namespace Aplicacao.Validadores;

public class ProdutoValidator : AbstractValidator<Produto>
{
    public ProdutoValidator()
    {
        RuleFor(p => p.Nome)
           .NotEmpty()
           .WithMessage("Campo Obrigatório")
           .WithName("Nome");

        RuleFor(p => p.Valor)
           .GreaterThan(0)
           .WithMessage("Valor deve ser maior que 0")
           .WithName("Valor");

        RuleFor(p => p.Descricao)
            .NotEmpty()
            .WithMessage("Campo obrigatório")
            .WithName("Descrição");
    }
}