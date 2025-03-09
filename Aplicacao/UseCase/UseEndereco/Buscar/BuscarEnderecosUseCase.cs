using AutoMapper;
using Dominio.Repositorios.Endereco;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseEndereco.Buscar;
public class BuscarEnderecosUseCase : IBuscarEnderecosUseCase
{
	private readonly IEnderecoReadOnly _enderecoReadOnly;
	private readonly IMapper _mapper;

	public BuscarEnderecosUseCase(IEnderecoReadOnly enderecoReadOnly, IMapper mapper)
	{
		_enderecoReadOnly = enderecoReadOnly;
		_mapper = mapper;
	}

	public async Task<IEnumerable<ResponseEnderecoJson>> Execute(long usuarioId)
	{
		var enderecos = await _enderecoReadOnly.GetByUsuarioId(usuarioId);

		return _mapper.Map<IEnumerable<ResponseEnderecoJson>>(enderecos);
	}
}
