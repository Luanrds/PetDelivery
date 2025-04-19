using Dominio.Repositorios.Endereco;
using Dominio.Repositorios;
using Dominio.Entidades;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseEndereco.Excluir;
public class ExcluirEnderecoUseCase : IExcluirEnderecoUseCase
{
	private readonly IEnderecoReadOnly _enderecoReadOnly;
	private readonly IEnderecoWriteOnly _enderecoWriteOnly;
	private readonly IUnitOfWork _unitOfWork;

	public ExcluirEnderecoUseCase(
		IEnderecoReadOnly enderecoReadOnly,
		IEnderecoWriteOnly enderecoWriteOnly,
		IUnitOfWork unitOfWork)
	{
		_enderecoReadOnly = enderecoReadOnly;
		_enderecoWriteOnly = enderecoWriteOnly;
		_unitOfWork = unitOfWork;
	}

	public async Task ExecuteAsync(long usuarioId, long enderecoId)
	{

		Endereco? endereco = await _enderecoReadOnly.GetById(usuarioId, enderecoId)
			?? throw new NotFoundException($"Endereço com ID {enderecoId} não encontrado para o usuário ID {usuarioId}.");

		_enderecoWriteOnly.Excluir(endereco); 

		await _unitOfWork.Commit();
	}
}
