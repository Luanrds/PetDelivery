using Dominio.Enums;

namespace PetDelivery.Communication.Response;
public class ResponseCartaoCreditoJson
{
	public long Id { get; set; }
	public string NomeTitular { get; set; } = string.Empty;
	public string NumeroCartaoUltimosQuatro { get; set; } = string.Empty;
	public string DataValidade { get; set; } = string.Empty;
	public TipoCartao Tipo { get; set; }
}
