using Dominio.Extensoes;
using FluentValidation;
using PetDelivery.Communication.Request;

namespace Aplicacao.UseCase.UseUsuario.Atualizar;
public class AtualizarUsuarioValidator : AbstractValidator<RequestAtualizarUsuarioJson>
{
    public AtualizarUsuarioValidator()
    {
		RuleFor(request => request.Nome)
			.NotEmpty()
			.WithMessage("O nome não pode ser vazio.");

		RuleFor(request => request.Email)
			.NotEmpty()
			.WithMessage("O email não pode ser vazio.");

		When(request => request.Email.NotEmpty(), () =>
		{
			RuleFor(request => request.Email)
			.EmailAddress()
			.WithMessage("O email está inválido.");
		});
	}
}
