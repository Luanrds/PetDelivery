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
		produto.ImagensIdentificadores = [];

		if (request.Imagens != null && request.Imagens.Count != 0)
		{
			foreach (var imagemFile in request.Imagens)
			{
				var fileStream = imagemFile.OpenReadStream();
				(var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

				if (isValidImage.IsFalse())
				{
					throw new ErrorOnValidationException([$"Arquivo '{imagemFile.FileName}' não é uma imagem válida."]);
				}

				var imagemIdentificador = $"{Guid.NewGuid()}{extension}";
				produto.ImagensIdentificadores.Add(imagemIdentificador);

				await _blobStorageService.Uploud(usuarioLogado, fileStream, imagemIdentificador);
			}
		}

		await _writeOnly.Add(produto);
		await _unitOfWork.Commit();

		var response = _mapper.Map<ResponseProdutoJson>(produto);
		if (produto.ImagensIdentificadores.Count != 0)
		{
			response.ImagemUrl = await _blobStorageService.GetFileUrl(usuarioLogado, produto.ImagensIdentificadores.First());
		}

		return response;
	}

	private static void Validate(RequestRegistroProdutoFormData request)
	{
		ProdutoValidator validator = new();
		ValidationResult result = validator.Validate(request);

		if (request is RequestRegistroProdutoFormData formData)
		{
			if (formData.Imagens != null)
			{
				foreach (var imagem in formData.Imagens)
				{
					if (imagem.Length == 0)
					{
						result.Errors.Add(new ValidationFailure(nameof(request.Imagens), $"O arquivo de imagem '{imagem.FileName}' está vazio."));
					}
				}
			}
		}

		if (result.IsValid == false)
		{
			List<string> mensagensDeErro = result.Errors.Select(e => e.ErrorMessage).ToList();
			throw new ErrorOnValidationException(mensagensDeErro);
		}
	}
}