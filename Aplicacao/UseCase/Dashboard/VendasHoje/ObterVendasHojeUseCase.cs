using Dominio.Entidades;
using Dominio.Repositorios.Pedido;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Dashboard.VendasHoje;
public class ObterVendasHojeUseCase : IObterVendasHojeUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IPedidoReadOnly _pedidoReadOnly;

	public ObterVendasHojeUseCase(IUsuarioLogado usuarioLogado, IPedidoReadOnly pedidoReadOnly)
	{
		_usuarioLogado = usuarioLogado;
		_pedidoReadOnly = pedidoReadOnly;
	}

	public async Task<ResponseVendasHojeJson> ExecuteAsync()
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		decimal vendasHoje = await _pedidoReadOnly.GetTotalVendasDeHojePorVendedorAsync(usuarioLogado.Id);
		decimal vendasOntem = await _pedidoReadOnly.GetTotalVendasDeOntemPorVendedorAsync(usuarioLogado.Id);

		decimal? variacaoPercentual = null;
		string mensagemComparacao;

		mensagemComparacao = GerarMensagemComparacaoVendas(vendasHoje, vendasOntem, ref variacaoPercentual);

		return new ResponseVendasHojeJson
		{
			ValorVendasHoje = vendasHoje,
			VariacaoOntemPercentual = variacaoPercentual.HasValue ? Math.Round(variacaoPercentual.Value, 2) : (decimal?)null,
			MensagemComparacao = mensagemComparacao
		};
	}

	private static string GerarMensagemComparacaoVendas(decimal vendasHoje, decimal vendasOntem, ref decimal? variacaoPercentual)
	{
		string mensagemComparacao;
		if (vendasOntem > 0)
		{
			variacaoPercentual = ((vendasHoje - vendasOntem) / vendasOntem) * 100;

			string direcao = variacaoPercentual >= 0 ? "aumento" : "redução";
			if (variacaoPercentual == 0)
			{
				mensagemComparacao = "mesmo valor de ontem";
			}
			else
			{
				mensagemComparacao = $"{Math.Abs(variacaoPercentual.Value):F0}% de {direcao} em relação a ontem";
			}
		}
		else if (vendasHoje > 0)
		{
			mensagemComparacao = "Não houve vendas ontem para comparar.";
		}
		else
		{
			mensagemComparacao = "Sem vendas hoje ou ontem.";
		}

		return mensagemComparacao;
	}
}
