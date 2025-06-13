using Aplicacao.Extensoes;
using Aplicacao.Validadores;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.Storage;
using Dominio.Servicos.UsuarioLogado;
using FluentValidation.Results;
using PetDelivery.Communication.Request;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.Atualizar;

public class AtualizeProdutoUseCase : IAtualizeProdutoUseCase
{
	private readonly IProdutoUpdateOnly _repositorioUpdate;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IBlobStorageService _blobStorageService;

	public AtualizeProdutoUseCase(
			IProdutoUpdateOnly repositorioUpdate,
			IUnitOfWork unitOfWork,
			IMapper mapper,
			IUsuarioLogado usuarioLogado,
			IBlobStorageService blobStorageService)
	{
		_repositorioUpdate = repositorioUpdate;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
		_blobStorageService = blobStorageService;
	}

	public async Task ExecuteAsync(long produtoId, RequestRegistroProdutoFormData requisicao)
	{
		Usuario vendedorLogado = await _usuarioLogado.Usuario();

		Validate(requisicao);

		Produto produto = await _repositorioUpdate.GetById(produtoId)
			?? throw new NotFoundException($"Produto com ID {produtoId} não encontrado.");

		if (produto.UsuarioId != vendedorLogado.Id)
		{
			throw new UnauthorizedException("Você não tem permissão para alterar este produto.");
		}

		_mapper.Map(requisicao, produto);

		if (requisicao.Imagens != null && requisicao.Imagens.Count > 0)
		{
			foreach (var imagemAntigaId in produto.ImagensIdentificadores)
			{
				await _blobStorageService.Excluir(vendedorLogado, imagemAntigaId);
			}

			produto.ImagensIdentificadores.Clear();

			foreach (var imagemFile in requisicao.Imagens)
			{
				var fileStream = imagemFile.OpenReadStream();
				(var isValidImage, var extension) = fileStream.ValidateAndGetImageExtension();

				if (isValidImage.IsFalse())
				{
					throw new ErrorOnValidationException([$"Arquivo '{imagemFile.FileName}' não é uma imagem válida."]);
				}

				var imagemIdentificador = $"{Guid.NewGuid()}{extension}";
				produto.ImagensIdentificadores.Add(imagemIdentificador);

				await _blobStorageService.Uploud(vendedorLogado, fileStream, imagemIdentificador);
			}
		}

		_repositorioUpdate.Atualize(produto);
		await _unitOfWork.Commit();
	}

	public static void Validate(RequestRegistroProdutoFormData requisicao)
	{
		ProdutoValidator validator = new();
		ValidationResult result = validator.Validate(requisicao);

		if (requisicao.Imagens != null)
		{
			foreach (var imagem in requisicao.Imagens)
			{
				if (imagem.Length == 0)
				{
					result.Errors.Add(new ValidationFailure(nameof(requisicao.Imagens), $"O arquivo de imagem '{imagem.FileName}' está vazio."));
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