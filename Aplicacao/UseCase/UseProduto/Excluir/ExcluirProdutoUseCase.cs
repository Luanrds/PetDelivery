using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.Excluir;
public class ExcluirProdutoUseCase : IExcluirProdutoUseCase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IProdutoReadOnly _repositoryRead;
	private readonly IProdutoWriteOnly _repositoryWrite;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IBlobStorageService _blobStorageService;

	public ExcluirProdutoUseCase(
		IProdutoReadOnly repositoryRead,
		IProdutoWriteOnly repositoryWrite,
		IUnitOfWork unitOfWork,
		IUsuarioLogado usuarioLogado,
		IBlobStorageService blobStorageService)
	{
		_repositoryRead = repositoryRead;
		_repositoryWrite = repositoryWrite;
		_unitOfWork = unitOfWork;
		_usuarioLogado = usuarioLogado;
		_blobStorageService = blobStorageService;
	}

	public async Task ExecuteAsync(long produtoId)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();
		Produto produto = await _repositoryRead.GetById(produtoId)
			?? throw new NotFoundException($"Produto com ID {produtoId} não encontrado.");

		if (produto.UsuarioId != usuarioLogado.Id)
		{
			throw new UnauthorizedException("Você não tem permissão para excluir este produto.");
		}

		if (produto.ImagensIdentificadores != null && produto.ImagensIdentificadores.Any())
		{
			foreach (var imagemIdentificador in produto.ImagensIdentificadores)
			{
				if (imagemIdentificador.NotEmpty())
				{
					await _blobStorageService.Excluir(usuarioLogado, imagemIdentificador);
				}
			}
		}

		await _repositoryWrite.Excluir(produtoId);
		await _unitOfWork.Commit();
	}
}
