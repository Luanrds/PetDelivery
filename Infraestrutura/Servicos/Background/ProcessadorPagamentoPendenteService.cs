using Dominio.Repositorios.Pedido;
using Dominio.Servicos.Pagamento;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Infraestrutura.Servicos.Background;
public class ProcessadorPagamentoPendenteService : BackgroundService
{
	private readonly ILogger<ProcessadorPagamentoPendenteService> _logger;
	private readonly IServiceProvider _serviceProvider; // Usado para criar escopos para serviços Scoped
	private readonly TimeSpan _periodoVerificacao;
	private readonly int _tamanhoLote;

	public ProcessadorPagamentoPendenteService(
		ILogger<ProcessadorPagamentoPendenteService> logger,
		IServiceProvider serviceProvider,
		IConfiguration configuration) // Injetar IConfiguration
	{
		_logger = logger;
		_serviceProvider = serviceProvider;

		// Ler configurações do appsettings.json (ou usar valores padrão)
		_periodoVerificacao = TimeSpan.FromSeconds(configuration.GetValue<int>("BackgroundServices:Pagamento:IntervaloSegundos", 30));
		_tamanhoLote = configuration.GetValue<int>("BackgroundServices:Pagamento:TamanhoLote", 10); // Processar 10 por vez
		_logger.LogInformation("ProcessadorPagamentoPendenteService configurado para rodar a cada {Intervalo} processando lotes de {TamanhoLote}.", _periodoVerificacao, _tamanhoLote);
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("Processador de Pagamentos Pendentes iniciado.");

		// Loop principal que roda enquanto a aplicação estiver ativa
		while (!stoppingToken.IsCancellationRequested)
		{
			try
			{
				_logger.LogDebug("Ciclo de verificação de pagamentos pendentes iniciado.");

				// **IMPORTANTE:** DbContext, Repositórios e UnitOfWork são 'Scoped'.
				// BackgroundService é 'Singleton'. Precisamos criar um escopo manualmente
				// para usar dependências Scoped dentro do ExecuteAsync.
				using (var scope = _serviceProvider.CreateScope())
				{
					// Resolve as dependências necessárias DENTRO do escopo atual
					var pedidoReadOnly = scope.ServiceProvider.GetRequiredService<IPedidoReadOnly>();
					var simuladorPagamento = scope.ServiceProvider.GetRequiredService<ISimuladorPagamentoService>(); // Este serviço usará seu próprio UoW/DbContext

					// Busca apenas os IDs dos pedidos que precisam ter o pagamento simulado
					var pedidosPendentesIds = await pedidoReadOnly.ObterIdsDePedidosComPagamentoPendenteAsync(_tamanhoLote, stoppingToken);

					if (pedidosPendentesIds.Any())
					{
						_logger.LogInformation("Encontrados {Count} pagamentos pendentes. Iniciando processamento do lote.", pedidosPendentesIds.Count);

						// Processa cada pedido encontrado individualmente
						foreach (var pedidoId in pedidosPendentesIds)
						{
							if (stoppingToken.IsCancellationRequested) break; // Verifica cancelamento antes de cada item

							try
							{
								_logger.LogInformation("Iniciando simulação de pagamento para Pedido ID: {PedidoId}", pedidoId);
								// Chama o serviço que contém a lógica de simulação e atualização do BD
								await simuladorPagamento.SimularConfirmacaoPagamentoAsync(pedidoId);
								_logger.LogInformation("Simulação de pagamento concluída para Pedido ID: {PedidoId}", pedidoId);
							}
							catch (Exception ex)
							{
								// Captura erro para um pedido específico, loga, e continua o loop para os outros
								_logger.LogError(ex, "Erro ao processar simulação de pagamento para Pedido ID: {PedidoId}. Este pedido será ignorado neste ciclo.", pedidoId);
								// Em um sistema real, poderia ter lógica de retentativa ou marcar o pedido com erro.
							}
						}
						_logger.LogInformation("Lote de {Count} pagamentos pendentes processado.", pedidosPendentesIds.Count);
					}
					else
					{
						_logger.LogDebug("Nenhum pagamento pendente encontrado neste ciclo.");
					}
				} // O escopo é descartado aqui, liberando DbContext e outros serviços Scoped
			}
			catch (Exception ex) when (ex is not OperationCanceledException) // Não logar erro se for cancelamento normal
			{
				// Erro inesperado no ciclo principal (ex: falha ao criar scope, erro na conexão inicial do BD)
				_logger.LogError(ex, "Erro crítico no loop do ProcessadorPagamentoPendenteService. O serviço tentará continuar no próximo ciclo.");
				// Em produção, pode ser necessário um mecanismo de alerta aqui.
			}

			// Espera o período configurado antes de iniciar o próximo ciclo
			try
			{
				await Task.Delay(_periodoVerificacao, stoppingToken);
			}
			catch (OperationCanceledException)
			{
				// A aplicação está parando, sair do loop
				break;
			}
		}

		_logger.LogInformation("Processador de Pagamentos Pendentes finalizando.");
	}
}
