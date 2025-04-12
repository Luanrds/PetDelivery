using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Usuario;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.UseUsuario.Login;
public class LoginUsuarioUseCase : ILoginUsuarioUseCase
{
	private readonly IUsuarioReadOnly _usuarioReadOnly;
	private readonly IMapper _mapper;

	public LoginUsuarioUseCase(IUsuarioReadOnly usuarioReadOnly, IMapper mapper)
	{
		_usuarioReadOnly = usuarioReadOnly;
		_mapper = mapper;
	}

	public async Task<ResponseUsuarioJson?> Execute(RequestLoginUsuarioJson request)
	{
		Usuario? usuario = await _usuarioReadOnly.GetByEmail(request.Email);

		if (usuario is null || usuario.Senha != request.Senha)
		{
			return null;
		}

		var response = _mapper.Map<ResponseUsuarioJson>(usuario);
		return response;
	}
}
