using FluentValidation;
using PetDelivery.Communication.Request;

namespace Aplicacao.Validadores;
public class UsuarioValidator : AbstractValidator<RequestRegistroDeUsuarioJson>
{
    public UsuarioValidator()
    {
		RuleFor(user => user.Nome)
			.NotEmpty()
			.WithMessage("Campo nome obrigatório.");

		RuleFor(user => user.Email)
			.NotEmpty()
			.WithMessage("Campo email obrigatório.");

		RuleFor(user => user.Senha)
			.NotEmpty()
			.WithMessage("Campo senha obrigatório");
	}
}
