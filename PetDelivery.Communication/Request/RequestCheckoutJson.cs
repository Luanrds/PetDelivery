using Dominio.Enums;

namespace PetDelivery.Communication.Request;

public class RequestCheckoutJson
{
	public long EnderecoId { get; set; }
	public MetodoPagamento MetodoPagamento { get; set; }
}