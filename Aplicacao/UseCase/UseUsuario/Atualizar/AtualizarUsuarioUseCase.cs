using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Repositorios;
using Dominio.Repositorios.Usuario;
using Dominio.Servicos.UsuarioLogado;
using FluentValidation.Results;
using PetDelivery.Communication.Request;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.Atualizar;
public class AtualizarUsuarioUseCase : IAtualizarUsuarioUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IUsuarioUpdateOnly _usuarioUpdateOnly;
	private readonly IUsuarioReadOnly _usuarioReadOnly;
	private readonly IUnitOfWork _unitOfWork;

	public AtualizarUsuarioUseCase(
		IUsuarioLogado usuarioLogado,
		IUsuarioUpdateOnly usuarioUpdateOnly,
		IUsuarioReadOnly usuarioReadOnly,
		IUnitOfWork unitOfWork)
	{
		_usuarioLogado = usuarioLogado;
		_usuarioUpdateOnly = usuarioUpdateOnly;
		_usuarioReadOnly = usuarioReadOnly;
		_unitOfWork = unitOfWork;
	}

	public async Task ExecuteAsync(RequestAtualizarUsuarioJson request)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		await Validate(request, usuarioLogado.Email);

		Usuario usuario = await _usuarioUpdateOnly.GetById(usuarioLogado.Id);

		usuario.Nome = request.Nome;
		usuario.Email = request.Email;

		_usuarioUpdateOnly.Atualize(usuario);

		await _unitOfWork.Commit();
	}

	private async Task Validate(RequestAtualizarUsuarioJson request, string emailAtual)
	{
		AtualizarUsuarioValidator validator = new();

		ValidationResult resultado = await validator.ValidateAsync(request);

		if (emailAtual.Equals(request.Email).IsFalse())
		{
			bool usuarioExiste = await _usuarioReadOnly.ExisteUsuarioAtivoComEmail(request.Email);
			if (usuarioExiste)
				resultado.Errors.Add(new ValidationFailure("Email", "Email já está registrado na plataforma"));
		}

		if (resultado.IsValid.IsFalse())
		{
			List<string> mensagensDeErro = resultado.Errors.Select(error => error.ErrorMessage).ToList();

			throw new ErrorOnValidationException(mensagensDeErro);
		}
	}
}
