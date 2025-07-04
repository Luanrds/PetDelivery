﻿namespace Dominio.Repositorios.Pedido;
public interface IPedidoReadOnly
{
	Task<Entidades.Pedido?> GetByIdAsync(long pedidoId);
	Task<IList<Entidades.Pedido>> GetByUsuarioIdAsync(long usuarioId);
	Task<decimal> GetTotalVendasDeHojePorVendedorAsync(long vendedorId);
	Task<decimal> GetTotalVendasDeOntemPorVendedorAsync(long vendedorId);
	Task<int> GetContagemNovosPedidosDeHojePorVendedorAsync(long vendedorId);
	Task<int> GetContagemNovosPedidosDeOntemPorVendedorAsync(long vendedorId);
	Task<IList<Entidades.Pedido>> GetUltimosPedidosContendoProdutosDoVendedorAsync(long vendedorId, int topN = 5);
	Task<IList<ObjetosDeValor.VendaMensalInfo>> GetVendasMensaisPorVendedorAsync(long vendedorId, DateTime dataInicio, DateTime dataFim);
	Task<bool> ProdutoJaVendido(long produtoId);
	Task<IList<ObjetosDeValor.ProdutoVendidoInfo>> ObterProdutosMaisVendidos(int limite);
}
