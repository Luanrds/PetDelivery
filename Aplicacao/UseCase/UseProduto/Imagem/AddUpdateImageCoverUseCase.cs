using Aplicacao.Extensoes;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using Microsoft.AspNetCore.Http;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.Imagem;
public class AddUpdateImageCoverUseCase : IAddUpdateImageCoverUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IProdutoUpdateOnly _produtoUpdateOnly;
	private readonly IProdutoReadOnly _produtoReadOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IBlobStorageService _blobStorageService;

	public AddUpdateImageCoverUseCase(
		IUsuarioLogado usuarioLogado,
		IProdutoUpdateOnly produtoUpdateOnly,
		IProdutoReadOnly produtoReadOnly,
		IUnitOfWork unitOfWork,
		IBlobStorageService blobStorageService)
	{
		_usuarioLogado = usuarioLogado;
		_produtoUpdateOnly = produtoUpdateOnly;
		_produtoReadOnly = produtoReadOnly;
		_unitOfWork = unitOfWork;
		_blobStorageService = blobStorageService;
	}

	public async Task Execute(long produtoId, IFormFile file)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();
		Produto? produto = await _produtoReadOnly.GetById(produtoId);

		if (produto is null || produto.UsuarioId != usuarioLogado.Id)
		{
			throw new NotFoundException($"Produto com ID {produtoId} não encontrado ou não pertence a este usuário.");
		}

		Stream fileStream = file.OpenReadStream();
		(var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

		if (isValidImage.IsFalse())
		{
			throw new ErrorOnValidationException(["O arquivo enviado não é uma imagem válida."]);
		}

		var novaImagemIdentificador = $"{Guid.NewGuid()}{extension}";

		produto.ImagensIdentificadores ??= [];
		produto.ImagensIdentificadores.Add(novaImagemIdentificador);

		_produtoUpdateOnly.Atualize(produto);
		await _unitOfWork.Commit();

		fileStream.Position = 0;
		await _blobStorageService.Uploud(usuarioLogado, fileStream, novaImagemIdentificador);
	}
}
