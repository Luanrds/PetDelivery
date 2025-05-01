using Microsoft.AspNetCore.Mvc;
using PetDelivery.API.Filtros;

namespace PetDelivery.API.Atributos;

public class UsuarioAutenticadoAttribute : TypeFilterAttribute
{
	public bool RequerVendedor { get; }

	public UsuarioAutenticadoAttribute() : base(typeof(UsuarioAutenticadoFilter))
	{
		RequerVendedor = false;
	}

	public UsuarioAutenticadoAttribute(bool requerVendedor) : base(typeof(UsuarioAutenticadoFilter))
	{
		RequerVendedor = requerVendedor;
		Arguments = [requerVendedor];
	}
}
