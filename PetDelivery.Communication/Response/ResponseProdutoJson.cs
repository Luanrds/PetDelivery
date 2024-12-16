namespace PetDelivery.Communication.Response;

public class ResponseProdutoJson
{
    public string Id { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public decimal Valor { get; set; } = decimal.Zero;

    public bool Disponivel { get; set; } = false;

    public string Descricao { get; set; } = string.Empty;
}
