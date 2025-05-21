using Aplicacao.Extensoes;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using Microsoft.AspNetCore.Http;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.Imagem;
public class AddUpdateImageCoverUseCase : IAddUpdateImageCoverUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IProdutoUpdateOnly _produtoUpdateOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IBlobStorageService _blobStorageService;

	public AddUpdateImageCoverUseCase(
		IUsuarioLogado usuarioLogado, 
		IProdutoUpdateOnly produtoUpdateOnly, 
		IUnitOfWork unitOfWork,
		IBlobStorageService blobStorageService)
    {
		_produtoUpdateOnly = produtoUpdateOnly;
		_unitOfWork = unitOfWork;
		_usuarioLogado = usuarioLogado;
		_blobStorageService = blobStorageService;
	}

    public async Task Execute(long produtoId, IFormFile file)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		Produto? produto = await _produtoUpdateOnly.GetById(produtoId) //Ajuste neste método para que ele passe como parametro o id do usuario logado
			?? throw new NotFoundException($"Produto não encontrado.");

		Stream fileStream = file.OpenReadStream();

		(var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

		if (isValidImage.IsFalse())
		{
			throw new ErrorOnValidationException(["Somente imagens"]);
		}

		if (string.IsNullOrEmpty(produto.ImagemIdentificador))
		{
			produto.ImagemIdentificador = $"{Guid.NewGuid()}{extension}";

			_produtoUpdateOnly.Atualize(produto);

			await _unitOfWork.Commit();
		}

		fileStream.Position = 0;

		await _blobStorageService.Uploud(usuarioLogado, fileStream, produto.ImagemIdentificador);
	}
}
