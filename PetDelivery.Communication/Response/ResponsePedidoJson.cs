using Dominio.Enums;

namespace PetDelivery.Communication.Response;
public class ResponsePedidoJson
{
	public long Id { get; set; }
	public long UsuarioId { get; set; }
	public long EnderecoId { get; set; }
	public StatusPedido Status { get; set; }
	public DateTime DataPedido { get; set; }
	public decimal ValorTotal { get; set; }

	public List<ResponseItemPedidoJson> Itens { get; set; } = [];
	public ResponseEnderecoJson? Endereco { get; set; }
	public ResponsePagamentoJson? Pagamento { get; set; }
}
