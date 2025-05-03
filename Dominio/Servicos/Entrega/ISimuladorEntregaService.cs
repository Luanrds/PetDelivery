namespace Dominio.Servicos.Entrega;
public interface ISimuladorEntregaService
{
	Task SimularEtapasEntregaAsync(long pedidoId);

}
