using Aplicacao.Validadores;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.UsuarioLogado;
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

	public ProdutoUseCase(
		IProdutoWriteOnly writeOnly,
		IUnitOfWork unitOfWork,
		IMapper mapper,
		IUsuarioLogado usuarioLogado)
	{
		_writeOnly = writeOnly;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_usuarioLogado = usuarioLogado;
	}

	public async Task<ResponseProdutoJson> ExecuteAsync(RequestProdutoJson request)
	{
		Usuario vendedorLogado = await _usuarioLogado.Usuario();

		Validate(request);

		Produto produto = _mapper.Map<Produto>(request);
		produto.UsuarioId = vendedorLogado.Id;

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