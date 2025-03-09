using AutoMapper;
using Dominio.Repositorios.Endereco;
using Dominio.Repositorios;
using PetDelivery.Communication.Request;
using PetDelivery.Exceptions.ExceptionsBase;
using Dominio.Entidades;

namespace Aplicacao.UseCase.UseEndereco.Atualizar;
public class AtualizeEnderecoUseCase : IAtualizeEnderecoUseCase
{
	private readonly IEnderecoReadOnly _enderecoReadOnly;
	private readonly IEnderecoWriteOnly _enderecoWriteOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public AtualizeEnderecoUseCase(
		IEnderecoReadOnly enderecoReadOnly,
		IEnderecoWriteOnly enderecoWriteOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper)
	{
		_enderecoReadOnly = enderecoReadOnly;
		_enderecoWriteOnly = enderecoWriteOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task Execute(long id, RequestAtualizarEnderecoJson request)
	{
		//Validar

		Endereco enderecoExistente = await _enderecoReadOnly.GetById(id) 
			?? throw new NotFoundException($"Endereço com ID {id} não encontrado.");

		_mapper.Map(request, enderecoExistente);

		_enderecoWriteOnly.Atualize(enderecoExistente);

		await _unitOfWork.Commit();
	}
}
