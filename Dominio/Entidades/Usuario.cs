﻿namespace Dominio.Entidades;
public class Usuario : EntidadeBase
{
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Senha { get; set; } = string.Empty; 
	public bool EhVendedor { get; set; } = false;
	public DateTime? DataNascimento { get; set; }
	public List<Endereco> Enderecos { get; set; } = [];
	public List<Pedido> Pedidos { get; set; } = [];
}
