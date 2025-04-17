using Aplicacao.Validadores;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Usuario;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.Criar;

public class UsuarioUseCase : IUsuarioUseCase
{
	private readonly IUsuarioWriteOnly _writeOnly;
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;

	public UsuarioUseCase(IUsuarioWriteOnly writeOnly, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _writeOnly = writeOnly;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

    public async Task<ResponseUsuarioJson> ExecuteAsync(RequestUsuarioRegistroJson request)
	{
		Validate(request);

		var usuario = _mapper.Map<Usuario>(request);

		await _writeOnly.Add(usuario);

		await _unitOfWork.Commit();

		return _mapper.Map<ResponseUsuarioJson>(usuario);
	}

	private static void Validate(RequestUsuarioRegistroJson request)
	{
		var validator = new UsuarioValidator();

		var result = validator.Validate(request);

		if (result.IsValid == false)
		{
			var mensagensDeErro = result.Errors.Select(e => e.ErrorMessage).ToList();

			throw new ErrorOnValidationException(mensagensDeErro);
		}
	}
}
