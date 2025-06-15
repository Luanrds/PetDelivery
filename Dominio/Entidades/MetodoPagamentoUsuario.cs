using Dominio.Enums;

namespace Dominio.Entidades;
public class MetodoPagamentoUsuario : EntidadeBase
{
    public long UsuarioId { get; set; }
    public string NomeTitular { get; set; } = string.Empty;
    public string NumeroCartao { get; set; } = string.Empty;
    public string DataValidade { get; set; } = string.Empty;
    public TipoCartao Tipo { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
