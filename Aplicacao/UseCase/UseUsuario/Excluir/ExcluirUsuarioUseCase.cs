using Dominio.Repositorios;
using Dominio.Repositorios.Usuario;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.Excluir;
public class ExcluirUsuarioUseCase : IExcluirUsuarioUseCase
{
	private readonly IUsuarioUpdateOnly _usuarioUpdateOnly;
	private readonly IUsuarioWriteOnly _usuarioWriteOnly;
	private readonly IUnitOfWork _unitOfWork;

	public ExcluirUsuarioUseCase(
		IUsuarioUpdateOnly usuarioUpdateOnly,
		IUsuarioWriteOnly usuarioWriteOnly,
		IUnitOfWork unitOfWork)
	{
		_usuarioUpdateOnly = usuarioUpdateOnly;
		_usuarioWriteOnly = usuarioWriteOnly;
		_unitOfWork = unitOfWork;
	}

	public async Task ExecuteAsync(long usuarioId)
	{
		_ = await _usuarioUpdateOnly.GetById(usuarioId)
			?? throw new NotFoundException($"Usuário com ID {usuarioId} não encontrado.");

		await _usuarioWriteOnly.Excluir(usuarioId);

		await _unitOfWork.Commit();
	}
}
