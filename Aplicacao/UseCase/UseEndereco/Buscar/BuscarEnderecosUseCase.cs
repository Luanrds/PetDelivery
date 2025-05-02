using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Endereco;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseEndereco.Buscar;
public class BuscarEnderecosUseCase : IBuscarEnderecosUseCase
{
	private readonly IEnderecoReadOnly _enderecoReadOnly;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado;

	public BuscarEnderecosUseCase(
		IEnderecoReadOnly enderecoReadOnly,
		IMapper mapper,
		IUsuarioLogado usuarioLogado)
	{
		_enderecoReadOnly = enderecoReadOnly;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
	}

	public async Task<IEnumerable<ResponseEnderecoJson>> ExecuteAsync()
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		IEnumerable<Endereco> enderecos = await _enderecoReadOnly.GetByUsuarioId(usuarioLogado.Id);

		return _mapper.Map<IEnumerable<ResponseEnderecoJson>>(enderecos);
	}
}
