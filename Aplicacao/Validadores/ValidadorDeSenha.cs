using FluentValidation;
using FluentValidation.Validators;

namespace Aplicacao.Validadores;
public class ValidadorDeSenha<T> : PropertyValidator<T, string>
{
	public override bool IsValid(ValidationContext<T> context, string senha)
	{
		if (string.IsNullOrWhiteSpace(senha))
		{
			context.MessageFormatter.AppendArgument("ErrorMessage", "A senha não pode estar vazia.");

			return false;
		}

		if (senha.Length < 6)
		{
			context.MessageFormatter.AppendArgument("ErrorMessage", "A senha deve ter no mínimo 6 caracteres.");

			return false;
		}

		return true;
	}

	public override string Name => "ValidadorDeSenha";

	protected override string GetDefaultMessageTemplate(string errorCode) => "{ErrorMessage}"; 
}

