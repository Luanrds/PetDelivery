using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PetDelivery.API.Filtros;

namespace PetDelivery.API.Atributos;

public class UsuarioAutenticadoAttribute : TypeFilterAttribute
{
    public UsuarioAutenticadoAttribute() : base(typeof(UsuarioAutenticadoFilter))
    {
    }
}
