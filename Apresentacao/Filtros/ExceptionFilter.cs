using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;
using System.Net;

namespace PetDelivery.API.Filtros;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is PetDeliveryExceptions petDeliveryExceptions)
        {
            HandleProjectException(petDeliveryExceptions, context);
        }
        else
        {
            ThrowUnknowException(context);
        }
    }

    private static void HandleProjectException(PetDeliveryExceptions petDeliveryExceptions, ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)petDeliveryExceptions.GetStatusCode();
        context.Result = new ObjectResult(new ResponseErrorJson(petDeliveryExceptions.GetMensagensDeErro()));
    }

    private static void ThrowUnknowException(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult(new ResponseErrorJson("Erro Desconhecido"));
    }
}
