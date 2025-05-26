using Dominio.Entidades;
using Dominio.Seguranca.Tokens;
using Dominio.Servicos.UsuarioLogado;
using Infraestrutura.Configuracao;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infraestrutura.Servicos.UsuarioLogado;
public class UsuarioLogado : IUsuarioLogado
{
	private readonly PetDeliveryDbContext _dbContext;
	private readonly ITokenProvider _tokenProvider;

	public UsuarioLogado(PetDeliveryDbContext dbContext, ITokenProvider tokenProvider)
	{
		_dbContext = dbContext;
		_tokenProvider = tokenProvider;
	}

	public async Task<Usuario> Usuario()
	{
		var token = _tokenProvider.Value();

		var tokenHandler = new JwtSecurityTokenHandler();

		var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

		var identificador = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

		var IdentificadorDoUsuario = Guid.Parse(identificador);

		return await _dbContext
			.Usuario
			.AsNoTracking()
			.FirstAsync(u => u.Ativo && u.IdentificadorDoUsuario == IdentificadorDoUsuario);
	}
}
