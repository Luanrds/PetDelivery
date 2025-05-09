using AutoMapper;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Repositorios;
using Dominio.Repositorios.Usuario;
using Dominio.Seguranca.Criptografia;
using Dominio.Servicos.UsuarioLogado;
using FluentValidation.Results;
using PetDelivery.Communication;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.AlterarSenha;
public class AlterarSenhaUsuarioUseCase : IAlterarSenhaUsuarioUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IUsuarioUpdateOnly _usuarioUpdateOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ISenhaEncripter _senhaEncripter;

    public AlterarSenhaUsuarioUseCase(
		IUsuarioLogado usuarioLogado,
		IUsuarioUpdateOnly usuarioUpdateOnly,
		IUnitOfWork unitOfWork,
		ISenhaEncripter senhaEncripter)
    {
		_usuarioLogado = usuarioLogado;
		_usuarioUpdateOnly = usuarioUpdateOnly;
		_unitOfWork = unitOfWork;
		_senhaEncripter = senhaEncripter;
	}

    public async Task ExecuteAsync(RequestAlterarSenhaUsuarioJson request)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		Validate(request, usuarioLogado);

		var usuario = await _usuarioUpdateOnly.GetById(usuarioLogado.Id);

		usuario.Senha = _senhaEncripter.Encrypt(request.NovaSenha);

		_usuarioUpdateOnly.Atualize(usuario);

		await _unitOfWork.Commit();
	}

	private void Validate(RequestAlterarSenhaUsuarioJson request, Usuario usuarioLogado)
	{
		var result = new AlterarSenhaValidator().Validate(request);

		if (_senhaEncripter.IsValid(request.Senha, usuarioLogado.Senha).IsFalse())
			result.Errors.Add(new ValidationFailure(string.Empty, "A senha inserida é diferente da senha atual."));

		if (result.IsValid.IsFalse())
			throw new ErrorOnValidationException(result.Errors.Select(e => e.ErrorMessage).ToList());
	}
}
