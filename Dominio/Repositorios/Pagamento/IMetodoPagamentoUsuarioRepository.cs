using Dominio.Entidades;

namespace Dominio.Repositorios.Pagamento;
public interface IMetodoPagamentoUsuarioRepository
{
	Task Adicionar(MetodoPagamentoUsuario metodoPagamento);
	Task<IList<MetodoPagamentoUsuario>> ObterPorUsuarioId(long usuarioId);
	Task<MetodoPagamentoUsuario?> ObterPorIdEUsuarioId(long id, long usuarioId);
	void Excluir(MetodoPagamentoUsuario metodoPagamento);
}
