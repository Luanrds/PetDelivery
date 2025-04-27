using Dominio.Extensoes;
using Dominio.Repositorios.Usuario;
using Dominio.Seguranca.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace PetDelivery.API.Filtros;

public class UsuarioAutenticadoFilter(IAccessTokenValidator accessTokenValidator, IUsuarioReadOnly usuarioReadOnly) : IAsyncAuthorizationFilter
{
	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		try
		{
			var token = TokenOnRequest(context);

			var identificadorUsuario = accessTokenValidator.ValidarEBuscarIdentificadorDoUsuario(token);

			var existe = await usuarioReadOnly.ExisteUsuarioAtivoComIdentificador(identificadorUsuario);
			if (existe.IsFalse())
			{
				throw new UnauthorizedException("Você não pode acessar este recurso");
			}
		}
		catch (SecurityTokenExpiredException)
		{
			context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("TokenIsExpired")
			{
				TokenIsExpired = true,
			});
		}
		catch (PetDeliveryExceptions petDeliveryException)
		{
			context.HttpContext.Response.StatusCode = (int)petDeliveryException.GetStatusCode();
			context.Result = new ObjectResult(new ResponseErrorJson(petDeliveryException.GetMensagensDeErro()));
		}
		catch
		{
			context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("Você não pode acessar este recurso"));
		}
	}

	private static string TokenOnRequest(AuthorizationFilterContext context)
	{
		var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
		if (string.IsNullOrWhiteSpace(authentication))
		{
			throw new UnauthorizedException("Sem Token");
		}

		return authentication["Bearer ".Length..].Trim();
	}
}
