using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Usuario;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.Buscar;
public class ObterUsuarioUseCase(IUsuarioReadOnly usuarioReadOnly, IMapper mapper) : IObterUsuarioUseCase
{
	private readonly IUsuarioReadOnly _usuarioReadOnly = usuarioReadOnly;
	private readonly IMapper _mapper = mapper;

	public async Task<ResponseUsuarioJson> ExecuteAsync(long id)
	{
		Usuario? usuario = await _usuarioReadOnly.GetById(id);

		return usuario == null
			? throw new NotFoundException($"Usuário com ID {id} não encontrado.")
			: _mapper.Map<ResponseUsuarioJson>(usuario);
	}
}
