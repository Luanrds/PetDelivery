using AutoMapper;
using Dominio.Repositorios.Usuario;
using Dominio.Repositorios;
using PetDelivery.Communication;
using PetDelivery.Exceptions.ExceptionsBase;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.AlterarSenha;
public class AlterarSenhaUsuarioUseCase : IAlterarSenhaUsuarioUseCase
{
	private readonly IUsuarioReadOnly _usuarioReadOnly;
	private readonly IUsuarioWriteOnly _usuarioWriteOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public AlterarSenhaUsuarioUseCase(
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

	public async Task Execute(long id, RequestAlterarSenhaUsuarioJson request)
	{
		//Validador 

		var usuarioExistente = await _usuarioReadOnly.GetById(id);

		if (usuarioExistente == null)
		{
			throw new NotFoundException($"Usuário com ID {id} não encontrado.");
		}

		usuarioExistente.Senha = request.NovaSenha;

		_usuarioWriteOnly.Atualize(usuarioExistente);

		await _unitOfWork.Commit();

		//_mapper.Map<ResponseUsuarioJson>(usuarioExistente);
	}
}
