using Aplicacao.Validadores;
using FluentValidation;
using PetDelivery.Communication;

namespace Aplicacao.UseCase.UseUsuario.AlterarSenha;
public class AlterarSenhaValidator : AbstractValidator<RequestAlterarSenhaUsuarioJson>
{
    public AlterarSenhaValidator()
    {
		RuleFor(x => x.NovaSenha)
			.SetValidator(new ValidadorDeSenha<RequestAlterarSenhaUsuarioJson>());
	}
}
