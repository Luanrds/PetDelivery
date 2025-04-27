using Dominio.Enums;

namespace PetDelivery.Communication.Request;

public class RequestCheckoutJson
{
	public long UsuarioId { get; set; }
	public long EnderecoId { get; set; }
	public MetodoPagamento MetodoPagamento { get; set; }
}