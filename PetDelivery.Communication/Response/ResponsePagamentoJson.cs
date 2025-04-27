using Dominio.Enums;

namespace PetDelivery.Communication.Response;
public class ResponsePagamentoJson
{
	public long Id { get; set; }
	public MetodoPagamento MetodoPagamento { get; set; }
	public StatusPagamento StatusPagamento { get; set; }
	public decimal Valor { get; set; }
}
