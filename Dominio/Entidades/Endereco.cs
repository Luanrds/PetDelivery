﻿namespace Dominio.Entidades;

public class Endereco : EntidadeBase
{
	public string Rua { get; set; } = string.Empty;
	public string Numero { get; set; } = string.Empty;
	public string Cidade { get; set; } = string.Empty;
	public string Estado { get; set; } = string.Empty;
	public string CEP { get; set; } = string.Empty;
}