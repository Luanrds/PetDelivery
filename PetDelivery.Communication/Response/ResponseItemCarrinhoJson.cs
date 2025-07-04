﻿namespace PetDelivery.Communication.Response;

public class ResponseItemCarrinhoJson
{
	public long Id { get; set; }
	public string Nome { get; set; } = string.Empty;
	public string Descricao { get; set; } = string.Empty;
	public string? ImagemUrl { get; set; }
	public int Quantidade { get; set; }
	public decimal PrecoUnitarioOriginal { get; set; }
	public decimal? PrecoUnitarioComDesconto { get; set; }
	public decimal? ValorDesconto { get; set; }
	public int? TipoDesconto { get; set; }
	public decimal SubTotal { get; set; }
	public long VendedorId { get; set; }
	public string NomeVendedor { get; set; } = string.Empty;
	public string NumeroVendedor { get; set; } = string.Empty;

}