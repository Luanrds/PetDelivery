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
			.NotEmpty()
			.WithMessage("Campo Obrigatório")
			.Must(ValidacoesDeSenha)
			.WithMessage("Senha inválida.");
	}

	private bool ValidacoesDeSenha(string senha)
	{
		if (string.IsNullOrEmpty(senha) || senha.Length < 6)
		{
			return false;
		}
		return true;
	}
}