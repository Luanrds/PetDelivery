using AutoMapper;
using Dominio.Repositorios;
using Dominio.Repositorios.Usuario;
using PetDelivery.Communication;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.AlterarSenha;
public class AlterarSenhaUsuarioUseCase : IAlterarSenhaUsuarioUseCase
{
	private readonly IUsuarioUpdateOnly _usuarioUpdateOnly;
	private readonly IUsuarioWriteOnly _usuarioWriteOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public AlterarSenhaUsuarioUseCase(
		IUsuarioUpdateOnly usuarioUpdateOnly,
		IUsuarioWriteOnly usuarioWriteOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper)
	{
		_usuarioUpdateOnly = usuarioUpdateOnly;
		_usuarioWriteOnly = usuarioWriteOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task ExecuteAsync(long id, RequestAlterarSenhaUsuarioJson request)
	{
		//Validador 

		var usuarioExistente = await _usuarioUpdateOnly.GetById(id);

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
