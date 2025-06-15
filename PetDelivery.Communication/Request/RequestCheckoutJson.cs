using Dominio.Enums;

namespace PetDelivery.Communication.Request;

public class RequestCheckoutJson
{
	public long EnderecoId { get; set; }
	public long? MetodoPagamentoUsuarioId { get; set; }
	public MetodoPagamento? MetodoPagamentoAvulso { get; set; }
}