using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Usuario;
using Dominio.Seguranca.Criptografia;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.Login;
public class LoginUsuarioUseCase : ILoginUsuarioUseCase
{
	private readonly IUsuarioReadOnly _usuarioReadOnly;
	private readonly ISenhaEncripter _senhaEncripter;
	private readonly IMapper _mapper;

	public LoginUsuarioUseCase(IUsuarioReadOnly usuarioReadOnly, ISenhaEncripter senhaEncripter, IMapper mapper)
	{
		_usuarioReadOnly = usuarioReadOnly;
		_senhaEncripter = senhaEncripter;
		_mapper = mapper;
	}

	public async Task<ResponseUsuarioJson?> ExecuteAsync(RequestLoginUsuarioJson request)
	{
		string senhaEncripitada = _senhaEncripter.Encrypt(request.Senha);

		Usuario? usuario = await _usuarioReadOnly.GetByEmailESenha(request.Email, senhaEncripitada)
			?? throw new LoginInvalidoException();

		var response = _mapper.Map<ResponseUsuarioJson>(usuario);
		return response;
	}
}
