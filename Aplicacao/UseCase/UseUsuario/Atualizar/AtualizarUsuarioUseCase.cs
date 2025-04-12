using AutoMapper;
using Dominio.Repositorios.Usuario;
using Dominio.Repositorios;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.Atualizar;
public class AtualizarUsuarioUseCase : IAtualizarUsuarioUseCase
{
	private readonly IUsuarioReadOnly _usuarioReadOnly;
	private readonly IUsuarioWriteOnly _usuarioWriteOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public AtualizarUsuarioUseCase(
	   IUsuarioReadOnly usuarioReadOnly,
	   IUsuarioWriteOnly usuarioWriteOnly,
	   IUnitOfWork unitOfWork,
	   IMapper mapper)
	{
		_usuarioReadOnly = usuarioReadOnly;
		_usuarioWriteOnly = usuarioWriteOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task Execute(long id, RequestAtualizarUsuarioJson request)
	{
		//Validate(request);

		var usuario = await _usuarioReadOnly.GetById(id) 
			?? throw new NotFoundException($"Usuário com ID {id} não encontrado.");

		_mapper.Map(request, usuario);

		_usuarioWriteOnly.Atualize(usuario);

		await _unitOfWork.Commit();
	}
}
