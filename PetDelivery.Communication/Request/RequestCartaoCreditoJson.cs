using Dominio.Enums;

namespace PetDelivery.Communication.Request;

public class RequestCartaoCreditoJson
{
	public string NomeTitular { get; set; } = string.Empty;
	public string NumeroCartao { get; set; } = string.Empty;
	public string DataValidade { get; set; } = string.Empty;
	public TipoCartao Tipo { get; set; }
}
