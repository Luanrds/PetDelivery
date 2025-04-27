using Aplicacao.Validadores;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Usuario;
using Dominio.Seguranca.Criptografia;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.Criar;

public class UsuarioUseCase : IUsuarioUseCase
{
	private readonly IUsuarioWriteOnly _writeOnly;
	private readonly IUsuarioReadOnly _readOnly;
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly ISenhaEncripter _senhaEncripter;

	public UsuarioUseCase(IUsuarioWriteOnly writeOnly, IUsuarioReadOnly readOnly, IMapper mapper, IUnitOfWork unitOfWork, ISenhaEncripter senhaEncripter)
	{
		_writeOnly = writeOnly;
		_readOnly = readOnly;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_senhaEncripter = senhaEncripter;
	}

	public async Task<ResponseUsuarioJson> ExecuteAsync(RequestUsuarioRegistroJson request)
	{
		await ValidateAsync(request);

		var usuario = _mapper.Map<Usuario>(request);
		usuario.Senha = _senhaEncripter.Encrypt(request.Senha);

		await _writeOnly.Add(usuario);

		await _unitOfWork.Commit();

		return _mapper.Map<ResponseUsuarioJson>(usuario);
	}

	private async Task ValidateAsync(RequestUsuarioRegistroJson request)
	{
		var validator = new UsuarioValidator();

		var result = validator.Validate(request);

		var emailExiste = await _readOnly.ExisteUsuarioComEmailAtivo(request.Email);

		if (emailExiste)
		{
			result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, "Email já registrado"));
		}

		if (result.IsValid == false)
		{
			var mensagensDeErro = result.Errors.Select(e => e.ErrorMessage).ToList();

			throw new ErrorOnValidationException(mensagensDeErro);
		}
	}
}
