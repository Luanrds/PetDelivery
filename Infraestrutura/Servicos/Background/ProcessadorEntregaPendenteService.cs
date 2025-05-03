using Dominio.Repositorios.Pedido;
using Dominio.Servicos.Entrega;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infraestrutura.Servicos.Background;
public class ProcessadorEntregaPendenteService : BackgroundService
{
	private readonly ILogger<ProcessadorEntregaPendenteService> _logger;
	private readonly IServiceProvider _serviceProvider;
	private readonly TimeSpan _periodoVerificacao;
	private readonly int _tamanhoLote;

	public ProcessadorEntregaPendenteService(
		ILogger<ProcessadorEntregaPendenteService> logger,
		IServiceProvider serviceProvider,
		IConfiguration configuration)
	{
		_logger = logger;
		_serviceProvider = serviceProvider;
		_periodoVerificacao = TimeSpan.FromSeconds(configuration.GetValue<int>("BackgroundServices:Entrega:IntervaloSegundos", 60)); // Verificar a cada 60 segundos
		_tamanhoLote = configuration.GetValue<int>("BackgroundServices:Entrega:TamanhoLote", 5); // Processar 5 por vez
		_logger.LogInformation("ProcessadorEntregaPendenteService configurado para rodar a cada {Intervalo} processando lotes de {TamanhoLote}.", _periodoVerificacao, _tamanhoLote);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Processador de Entregas Pendentes iniciado.");

		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				_logger.LogDebug("Ciclo de verificação de entregas pendentes iniciado.");
				using (var scope = _serviceProvider.CreateScope())
				{
					var pedidoReadOnly = scope.ServiceProvider.GetRequiredService<IPedidoReadOnly>();
					var simuladorEntrega = scope.ServiceProvider.GetRequiredService<ISimuladorEntregaService>();

					// Busca IDs de pedidos prontos para envio ou já enviados
					var pedidosParaProcessarIds = await pedidoReadOnly.ObterIdsDePedidosParaProcessarEntregaAsync(_tamanhoLote, stoppingToken);

					if (pedidosParaProcessarIds.Any())
					{
						_logger.LogInformation("Encontrados {Count} pedidos para simulação de entrega. Iniciando processamento do lote.", pedidosParaProcessarIds.Count);
						foreach (var pedidoId in pedidosParaProcessarIds)
						{
							if (stoppingToken.IsCancellationRequested) break;
							try
							{
								_logger.LogInformation("Iniciando simulação de entrega para Pedido ID: {PedidoId}", pedidoId);
								await simuladorEntrega.SimularEtapasEntregaAsync(pedidoId);
								_logger.LogInformation("Simulação de entrega concluída para Pedido ID: {PedidoId}", pedidoId);
							}
							catch (Exception ex)
							{
								_logger.LogError(ex, "Erro ao processar simulação de entrega para Pedido ID: {PedidoId}. Ignorando para este ciclo.", pedidoId);
							}
						}
						_logger.LogInformation("Lote de {Count} entregas processado.", pedidosParaProcessarIds.Count);
					}
					else
					{
						_logger.LogDebug("Nenhuma entrega pendente encontrada neste ciclo.");
					}
				}
			}
			catch (Exception ex) when (ex is not OperationCanceledException)
			{
				_logger.LogError(ex, "Erro crítico no loop do ProcessadorEntregaPendenteService.");
			}

			try
			{
				await Task.Delay(_periodoVerificacao, stoppingToken);
			}
			catch (OperationCanceledException)
			{
				break;
			}
		}
		_logger.LogInformation("Processador de Entregas Pendentes finalizando.");
	}
}