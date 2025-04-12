using Dominio.Repositorios;
using Dominio.Repositorios.Usuario;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.Excluir;
public class ExcluirUsuarioUseCase : IExcluirUsuarioUseCase
{
	private readonly IUsuarioReadOnly _usuarioReadOnly;
	private readonly IUsuarioWriteOnly _usuarioWriteOnly;
	private readonly IUnitOfWork _unitOfWork;

	public ExcluirUsuarioUseCase(
		IUsuarioReadOnly usuarioReadOnly,
		IUsuarioWriteOnly usuarioWriteOnly,
		IUnitOfWork unitOfWork)
	{
		_usuarioReadOnly = usuarioReadOnly;
		_usuarioWriteOnly = usuarioWriteOnly;
		_unitOfWork = unitOfWork;
	}

	public async Task Execute(long usuarioId)
	{
		_ = await _usuarioReadOnly.GetById(usuarioId)
			?? throw new NotFoundException($"Usuário com ID {usuarioId} não encontrado.");

		await _usuarioWriteOnly.Excluir(usuarioId);

		await _unitOfWork.Commit();
	}
}
