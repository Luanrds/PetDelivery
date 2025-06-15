using FluentValidation;
using PetDelivery.Communication.Request;

namespace Aplicacao.Validadores;

public class ProdutoValidator : AbstractValidator<RequestProdutoJson>
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
			.MaximumLength(5000)
			.WithMessage("Campo obrigatório")
            .WithName("Descrição");

		RuleFor(p => p.DescricaoResumida)
			.MaximumLength(500).WithMessage("A descrição resumida não pode exceder 500 caracteres.");

		RuleFor(p => p.Categoria)
            .IsInEnum()
            .WithMessage("Categoria inválida.");

		RuleFor(p => p.QuantidadeEstoque)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantidade em estoque não pode ser negativa.");
	}
}
