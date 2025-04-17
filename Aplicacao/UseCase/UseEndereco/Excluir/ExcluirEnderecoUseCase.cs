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

	public async Task ExecuteAsync(long id)
	{
		//validar

		Endereco endereco = await _enderecoReadOnly.GetById(id) 
			?? throw new NotFoundException($"Endereço com ID {id} não encontrado.");

		_enderecoWriteOnly.Excluir(endereco);

		await _unitOfWork.Commit();
	}
}
