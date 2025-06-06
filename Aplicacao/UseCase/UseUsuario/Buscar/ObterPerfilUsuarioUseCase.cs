using AutoMapper;
using Dominio.Entidades;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.Buscar;
public class ObterPerfilUsuarioUseCase(IMapper mapper, IUsuarioLogado usuarioLogado) : IObterPerfilUsuarioUseCase
{
	private readonly IUsuarioLogado _usuarioLogado = usuarioLogado;
	private readonly IMapper _mapper = mapper;

	public async Task<ResponsePerfilUsuarioJson> ExecuteAsync()
	{
		Usuario? usuario = await _usuarioLogado.Usuario();

		return _mapper.Map<ResponsePerfilUsuarioJson>(usuario);
	}
}
