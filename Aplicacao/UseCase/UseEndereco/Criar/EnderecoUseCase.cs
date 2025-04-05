using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Endereco;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseEndereco.Criar;
public class EnderecoUseCase : IEnderecoUseCase
{
	private readonly IEnderecoWriteOnly _enderecoWriteOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public EnderecoUseCase(
		IEnderecoWriteOnly enderecoWriteOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper)
	{
		_enderecoWriteOnly = enderecoWriteOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<ResponseEnderecoJson> Execute(RequestEnderecoJson request)
	{
		//validar

		var endereco = _mapper.Map<Endereco>(request);

		await _enderecoWriteOnly.Add(endereco);

		await _unitOfWork.Commit();

		return _mapper.Map<ResponseEnderecoJson>(endereco);
	}
}
