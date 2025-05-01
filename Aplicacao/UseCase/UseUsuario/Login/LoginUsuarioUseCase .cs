using AutoMapper;
using Dominio.Extensoes;
using Dominio.Repositorios.Usuario;
using Dominio.Seguranca.Criptografia;
using Dominio.Seguranca.Tokens;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseUsuario.Login;
public class LoginUseCase : ILoginUseCase
{
	private readonly IUsuarioReadOnly _usuarioReadOnly;
	private readonly ISenhaEncripter _senhaEncripter;
	private readonly IAccessTokenGenerator _accessTokenGenerator;
	private readonly IMapper _mapper;

	public LoginUseCase(IUsuarioReadOnly usuarioReadOnly,
		ISenhaEncripter senhaEncripter, IAccessTokenGenerator
		accessTokenGenerator,
		IMapper mapper)
	{
		_usuarioReadOnly = usuarioReadOnly;
		_senhaEncripter = senhaEncripter;
		_accessTokenGenerator = accessTokenGenerator;
		_mapper = mapper;
	}

	public async Task<ResponseUsuarioJson?> ExecuteAsync(RequestLoginUsuarioJson request)
	{
		var usuario = await _usuarioReadOnly.GetByEmail(request.Email);

		if (usuario is null || _senhaEncripter.IsValid(request.Senha, usuario.Senha).IsFalse())
			throw new LoginInvalidoException();

		return new ResponseUsuarioJson
		{
			Nome = usuario.Nome,
			EhVendedor = usuario.EhVendedor,
			Tokens = new ResponseTokensJson
			{
				AccessToken = _accessTokenGenerator.Gererate(usuario.IdentificadorDoUsuario)
			}
		};
	}
}
