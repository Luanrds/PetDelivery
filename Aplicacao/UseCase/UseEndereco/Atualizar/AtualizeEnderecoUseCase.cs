using AutoMapper;
using Dominio.Repositorios.Endereco;
using Dominio.Repositorios;
using PetDelivery.Communication.Request;
using PetDelivery.Exceptions.ExceptionsBase;
using Dominio.Entidades;
using Dominio.Servicos.UsuarioLogado;

namespace Aplicacao.UseCase.UseEndereco.Atualizar;
public class AtualizeEnderecoUseCase : IAtualizeEnderecoUseCase
{
	private readonly IEnderecoReadOnly _enderecoReadOnly;
	private readonly IEnderecoWriteOnly _enderecoWriteOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado;

	public AtualizeEnderecoUseCase(
		IEnderecoReadOnly enderecoReadOnly,
		IEnderecoWriteOnly enderecoWriteOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper,
		IUsuarioLogado usuarioLogado)
	{
		_enderecoReadOnly = enderecoReadOnly;
		_enderecoWriteOnly = enderecoWriteOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
	}

	public async Task ExecuteAsync(long enderecoId, RequestAtualizarEnderecoJson request)
	{
		var usuarioLogado = await _usuarioLogado.Usuario();

		// TODO: Adicionar validação para o RequestAtualizarEnderecoJson
		// Validate(request);

		Endereco? enderecoExistente = await _enderecoReadOnly.GetById(usuarioLogado.Id, enderecoId);

		if (enderecoExistente is null || enderecoExistente.UsuarioId != usuarioLogado.Id)
		{
			throw new NotFoundException($"Endereço com ID {enderecoId} não encontrado ou não pertence a este usuário.");
		}

		_mapper.Map(request, enderecoExistente);

		_enderecoWriteOnly.Atualize(enderecoExistente);

		await _unitOfWork.Commit();
	}
}
