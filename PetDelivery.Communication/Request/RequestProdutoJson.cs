namespace PetDelivery.Communication.Request;

public class RequestProdutoJson
{
    public string Nome { get; set; } = string.Empty;

    public decimal Valor { get; set; } = decimal.Zero;

    public bool Disponivel { get; set; } = false;

    public string Descricao { get; set; } = string.Empty;
}
