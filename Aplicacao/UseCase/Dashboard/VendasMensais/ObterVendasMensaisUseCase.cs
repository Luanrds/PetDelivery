using Dominio.Entidades;
using Dominio.ObjetosDeValor;
using Dominio.Repositorios.Pedido;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;
using System.Globalization;

namespace Aplicacao.UseCase.Dashboard.VendasMensais;

public class ObterVendasMensaisUseCase : IObterVendasMensaisUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IPedidoReadOnly _pedidoReadOnly;

	public ObterVendasMensaisUseCase(
		IUsuarioLogado usuarioLogado,
		IPedidoReadOnly pedidoReadOnly)
	{
		_usuarioLogado = usuarioLogado;
		_pedidoReadOnly = pedidoReadOnly;
	}

	public async Task<ResponseVendasMensaisGraficoJson> ExecuteAsync(string periodo)
	{

		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		(DateTime dataInicioUtc, DateTime dataFimUtc) = CalcularPeriodoUtc(periodo);

		IList<VendaMensalInfo> vendasDb = await _pedidoReadOnly.GetVendasMensaisPorVendedorAsync(usuarioLogado.Id, dataInicioUtc, dataFimUtc);

		ResponseVendasMensaisGraficoJson response = new();

		ResponseVendasMensaisDadosSerieJson serieVendas = new()
		{
			Nome = "Vendas Totais"
		};

		DateTime mesCorrente = dataInicioUtc;
		while (mesCorrente.Date <= dataFimUtc.Date)
		{
			string nomeMes = $"{DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(mesCorrente.Month)}/{mesCorrente.Year}";
			response.Categorias.Add(nomeMes);

			VendaMensalInfo? vendaDoMes = vendasDb.FirstOrDefault(v => v.Ano == mesCorrente.Year && v.Mes == mesCorrente.Month);
			serieVendas.Dados.Add(vendaDoMes?.TotalVendas ?? 0);

			if (mesCorrente.Month == 12)
			{
				mesCorrente = new DateTime(mesCorrente.Year + 1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			}
			else
			{
				mesCorrente = new DateTime(mesCorrente.Year, mesCorrente.Month + 1, 1, 0, 0, 0, DateTimeKind.Utc);
			}
		}

		response.Series.Add(serieVendas);

		return response;
	}

	private static (DateTime dataInicioUtc, DateTime dataFimUtc) CalcularPeriodoUtc(string periodoString)
	{
		DateTime hojeUtc = DateTime.UtcNow;

		DateTime dataInicioUtc;

		DateTime fimDoMesAtualUtc = new(hojeUtc.Year, hojeUtc.Month, DateTime.DaysInMonth(hojeUtc.Year, hojeUtc.Month), 0, 0, 0, DateTimeKind.Utc);


		dataInicioUtc = periodoString.ToLower() switch
		{
			"ultimos-3-meses" => new DateTime(hojeUtc.Year, hojeUtc.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-2),
			"ultimos-6-meses" => new DateTime(hojeUtc.Year, hojeUtc.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-5),
			"este-ano" => new DateTime(hojeUtc.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc),
			_ => new DateTime(hojeUtc.Year, hojeUtc.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-5),
		};

		return (dataInicioUtc, fimDoMesAtualUtc);
	}
}