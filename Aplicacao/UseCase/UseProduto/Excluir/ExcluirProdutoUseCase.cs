using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.Excluir;
public class ExcluirProdutoUseCase : IExcluirProdutoUseCase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IProdutoReadOnly _repositoryRead;
	private readonly IProdutoWriteOnly _repositoryWrite;
	private readonly IUsuarioLogado _usuarioLogado;

	public ExcluirProdutoUseCase(
		IProdutoReadOnly repositoryRead,
		IProdutoWriteOnly repositoryWrite,
		IUnitOfWork unitOfWork,
		IUsuarioLogado usuarioLogado)
	{
		_repositoryRead = repositoryRead;
		_repositoryWrite = repositoryWrite;
		_unitOfWork = unitOfWork;
		_usuarioLogado = usuarioLogado;
	}

	public async Task ExecuteAsync(long produtoId)
	{
		Usuario vendedorLogado = await _usuarioLogado.Usuario();

		Produto produto = await _repositoryRead.GetById(produtoId)
			?? throw new NotFoundException($"Produto com ID {produtoId} não encontrado.");

		if (produto.UsuarioId != vendedorLogado.Id)
		{
			throw new UnauthorizedException("Você não tem permissão para excluir este produto.");
		}

		await _repositoryWrite.Excluir(produtoId);

		await _unitOfWork.Commit();
	}
}
