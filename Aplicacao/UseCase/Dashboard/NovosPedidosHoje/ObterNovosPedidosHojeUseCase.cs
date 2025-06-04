using Dominio.Entidades;
using Dominio.Repositorios.Pedido;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Dashboard.NovosPedidosHoje;
public class ObterNovosPedidosHojeUseCase : IObterNovosPedidosHojeUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IPedidoReadOnly _pedidoReadOnly;

	public ObterNovosPedidosHojeUseCase(IUsuarioLogado usuarioLogado, IPedidoReadOnly pedidoReadOnly)
	{
		_usuarioLogado = usuarioLogado;
		_pedidoReadOnly = pedidoReadOnly;
	}

	public async Task<ResponseNovosPedidosHojeJson> ExecuteAsync()
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		int pedidosHojeCount =
			await _pedidoReadOnly.GetContagemNovosPedidosDeHojePorVendedorAsync(usuarioLogado.Id);

		int pedidosOntemCount =
			await _pedidoReadOnly.GetContagemNovosPedidosDeOntemPorVendedorAsync(usuarioLogado.Id);

		decimal? variacaoPercentual = null;
		string mensagemComparacao;

		if (pedidosOntemCount > 0)
		{
			variacaoPercentual = ((decimal)(pedidosHojeCount - pedidosOntemCount) / pedidosOntemCount) * 100;
			string direcao = variacaoPercentual >= 0 ? "aumento" : "redução";
			if (variacaoPercentual == 0)
			{
				mensagemComparacao = "mesmo número de ontem";
			}
			else
			{
				mensagemComparacao = $"{Math.Abs(variacaoPercentual.Value):F0}% de {direcao} em relação a ontem";
			}
		}
		else if (pedidosHojeCount > 0)
		{
			mensagemComparacao = "Não houve novos pedidos ontem para comparar.";
		}
		else
		{
			mensagemComparacao = "Sem novos pedidos hoje ou ontem.";
		}

		return new ResponseNovosPedidosHojeJson
		{
			QuantidadeNovosPedidos = pedidosHojeCount,
			VariacaoOntemPercentual = variacaoPercentual.HasValue ? Math.Round(variacaoPercentual.Value, 2) : null,
			MensagemComparacao = mensagemComparacao
		};
	}
}
