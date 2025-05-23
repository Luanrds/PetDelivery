using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Endereco;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseEndereco.Criar;
public class EnderecoUseCase : IEnderecoUseCase
{
	private readonly IEnderecoWriteOnly _enderecoWriteOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado;

	public EnderecoUseCase(
		IEnderecoWriteOnly enderecoWriteOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper,
		IUsuarioLogado usuarioLogado)
	{
		_enderecoWriteOnly = enderecoWriteOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
	}

	public async Task<ResponseEnderecoJson> ExecuteAsync(RequestEnderecoJson request)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		// TODO: Adicionar validação para o RequestEnderecoJson
		// Validate(request);

		Endereco endereco = _mapper.Map<Endereco>(request);

		endereco.UsuarioId = usuarioLogado.Id;

		await _enderecoWriteOnly.Add(endereco);
		await _unitOfWork.Commit();

		return _mapper.Map<ResponseEnderecoJson>(endereco);
	}
}
