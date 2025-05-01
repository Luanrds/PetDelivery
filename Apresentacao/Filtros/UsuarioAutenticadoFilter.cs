using Dominio.Extensoes;
using Dominio.Repositorios.Usuario;
using Dominio.Seguranca.Tokens;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace PetDelivery.API.Filtros;

public class UsuarioAutenticadoFilter(IAccessTokenValidator accessTokenValidator, IUsuarioReadOnly usuarioReadOnly, bool requerVendedor) : IAsyncAuthorizationFilter
{
	public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
	{
		try
		{
			var token = TokenOnRequest(context);

			var identificadorUsuario = accessTokenValidator.ValidarEBuscarIdentificadorDoUsuario(token);

			var usuario = await usuarioReadOnly.GetByIdentificador(identificadorUsuario);

			if (usuario is null || !usuario.Ativo)
			{
				throw new UnauthorizedException("Você não pode acessar este recurso");
			}

			if (requerVendedor)
			{
				if (usuario.EhVendedor.IsFalse())
				{
					throw new ForbiddenException("Acesso negado. Permissão de vendedor necessária.");
				}
			}
		}
		catch (SecurityTokenExpiredException)
		{
			context.Result = new UnauthorizedObjectResult(new ResponseErrorJson("Token expirado.")
			{
				TokenIsExpired = true,
			});
		}
		catch (ForbiddenException forbiddenException)
		{
			context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
			context.Result = new ObjectResult(new ResponseErrorJson(forbiddenException.Message));
		}
		catch (PetDeliveryExceptions petDeliveryException)
		{
			context.HttpContext.Response.StatusCode = (int)petDeliveryException.GetStatusCode();
			context.Result = new ObjectResult(new ResponseErrorJson(petDeliveryException.GetMensagensDeErro()));
		}
		catch (Exception ex)
		{
			context.Result = new UnauthorizedObjectResult(new ResponseErrorJson($"Erro de autenticação: {ex.Message}"));
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
