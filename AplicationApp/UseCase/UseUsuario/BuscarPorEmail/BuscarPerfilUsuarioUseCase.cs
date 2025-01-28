using AutoMapper;
using Dominio.Repositorios.Usuario;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.BuscarPorEmail;
public class BuscarPerfilUsuarioUseCase : IBucarPerfilUsuarioUseCase
{
	private readonly IMapper _mapper;
	private readonly IUsuarioReadOnlyRepository _repository;

	public BuscarPerfilUsuarioUseCase(IMapper mapper, IUsuarioReadOnlyRepository repository)
    {
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<ResponsePerfilUsuario> Execute(long usuarioId)
	{
		var usuario = await _repository.GetById(usuarioId);

		if (usuario is null)
		{
			throw new NotFoundException("Produto não encontrado.");
		}

		var response = _mapper.Map<ResponsePerfilUsuario>(usuario);

		return response;
	}
}
