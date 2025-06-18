namespace Dominio.Entidades;
public class Usuario : EntidadeBase
{
	public bool Ativo { get; set; } = true;
	public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
	public string Nome { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Senha { get; set; } = string.Empty;
	public string Telefone { get; set; } = string.Empty;
	public Guid IdentificadorDoUsuario { get; set; } = Guid.NewGuid();
	public bool EhVendedor { get; set; } = false;
	//public DateTime? DataNascimento { get; set; }
	public List<Endereco> Enderecos { get; set; } = [];
	public List<Pedido> Pedidos { get; set; } = [];
}
