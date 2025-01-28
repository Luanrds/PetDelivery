using Aplicacao.Validadores;
using AutoMapper;
using Dominio.Repositorios;
using Dominio.Repositorios.Usuario;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UserUseCase;
public class RegistroDeUsuarioUseCase : IRegistroDeUsuarioUseCase
{
	private readonly IMapper _mapper;
	private readonly IUsuarioWriteOnlyRepository _writeOnlyRepository;
	private readonly IUsuarioReadOnlyRepository _readOnlyRepository;
	private readonly IUnitOfWork _unitOfWork;

	public RegistroDeUsuarioUseCase(
		IUsuarioWriteOnlyRepository writeOnlyRepository,
		IUsuarioReadOnlyRepository readOnlyRepository,
		IUnitOfWork unitOfWork, 
		IMapper mapper)
    {
		_writeOnlyRepository = writeOnlyRepository;
		_readOnlyRepository = readOnlyRepository;
		_unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseUsuarioRegistradoJson> Execute(RequestRegistroDeUsuarioJson request)
	{
		Validate(request);

		var usuario = _mapper.Map<Dominio.Entidades.Usuario>(request);

		//Criptografia de senha

		await _writeOnlyRepository.Add(usuario);

		await _unitOfWork.Commit();

		return new ResponseUsuarioRegistradoJson
		{
			Nome = usuario.Nome
		};
	}

	private static void Validate(RequestRegistroDeUsuarioJson request)
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
