using Dominio.Seguranca.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infraestrutura.Seguranca.Tokens.Access.Generator;
public class JwtTokenGenerator : JwtTokenHandler, IAccessTokenGenerator
{
	private readonly uint _expirationTimeMinutes;
	private readonly string _signingKey;

	public JwtTokenGenerator(uint expirationTimeMinutes, string signingKey)
	{
		_expirationTimeMinutes = expirationTimeMinutes;
		_signingKey = signingKey;
	}

	public string Gererate(Guid IdentificadorDoUsuario)
	{
		var claims = new List<Claim>()
		{
			new(ClaimTypes.Sid, IdentificadorDoUsuario.ToString())
		};

		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
			SigningCredentials = new SigningCredentials(SecurityKey(_signingKey), SecurityAlgorithms.HmacSha256Signature)
		};

		var tokenHandler = new JwtSecurityTokenHandler();

		var securityToken = tokenHandler.CreateToken(tokenDescriptor);

		return tokenHandler.WriteToken(securityToken);
	}
}
