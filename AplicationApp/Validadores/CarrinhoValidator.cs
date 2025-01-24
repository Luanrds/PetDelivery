using FluentValidation;
using PetDelivery.Communication.Request;

namespace Aplicacao.Validadores;
public class CarrinhoValidator : AbstractValidator<RequestItemCarrinhoJson>
{
    public CarrinhoValidator() 
    {
		RuleFor(q => q.Quantidade)
			.GreaterThan(0)
			.WithMessage("Quantidade deve ser maior que 0")
			.WithName("Quantidade");
	}
}
