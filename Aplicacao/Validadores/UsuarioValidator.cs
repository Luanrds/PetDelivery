using FluentValidation;
using PetDelivery.Communication.Request;

namespace Aplicacao.Validadores;
public class UsuarioValidator : AbstractValidator<RequestUsuarioRegistroJson>
{
	public UsuarioValidator()
	{
		RuleFor(u => u.Nome)
		   .NotEmpty()
		   .WithMessage("Campo Obrigatório")
		   .WithName("Nome");

		RuleFor(p => p.Email)
		   .NotEmpty()
		   .WithMessage("Campo Obrigatório")
		   .WithName("Email");

		RuleFor(u => u.Senha)
			.SetValidator(new ValidadorDeSenha<RequestUsuarioRegistroJson>());
	}
}