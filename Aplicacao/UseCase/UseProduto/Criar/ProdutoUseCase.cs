using Aplicacao.Extensoes;
using Aplicacao.Validadores;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using FileTypeChecker.Extensions;
using FileTypeChecker.Types;
using FluentValidation.Results;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.Criar;

public class ProdutoUseCase : IProdutoUseCase
{
	private readonly IProdutoWriteOnly _writeOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado; 
	private readonly IBlobStorageService _blobStorageService;

	public ProdutoUseCase(
		IProdutoWriteOnly writeOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper,
		IUsuarioLogado usuarioLogado,
		IBlobStorageService blobStorageService)
	{
		_writeOnly = writeOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
		_blobStorageService = blobStorageService;
	}

	public async Task<ResponseProdutoJson> ExecuteAsync(RequestRegistroProdutoFormData request)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		Validate(request);

		Produto produto = _mapper.Map<Produto>(request);
		produto.UsuarioId = usuarioLogado.Id;

		if(request.Imagem is not null)
		{
			var fileStream = request.Imagem.OpenReadStream();

			(var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

			if (isValidImage.IsFalse())
			{
				throw new ErrorOnValidationException(["Somente Imagens"]);
			}

			produto.ImagemIdentificador = $"{Guid.NewGuid()}{extension}";

			await _blobStorageService.Uploud(usuarioLogado, fileStream, produto.ImagemIdentificador);
		}

		await _writeOnly.Add(produto);
		await _unitOfWork.Commit();

		return _mapper.Map<ResponseProdutoJson>(produto);
	}

	private static void Validate(RequestProdutoJson request)
	{
		ProdutoValidator validator = new();

		ValidationResult result = validator.Validate(request);

		if (result.IsValid == false)
		{
			List<string> mensagensDeErro = result.Errors.Select(e => e.ErrorMessage).ToList();

			throw new ErrorOnValidationException(mensagensDeErro);
		}
	}
}