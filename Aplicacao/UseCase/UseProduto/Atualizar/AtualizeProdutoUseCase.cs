using Aplicacao.Validadores;
using AutoMapper;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using PetDelivery.Communication.Request;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.Atualizar;
public class AtualizeProdutoUseCase : IAtualizeProdutoUseCase
{
	private readonly IProdutoUpdateOnly _repositorio;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

    public AtualizeProdutoUseCase(
		IProdutoUpdateOnly repositorio, 
		IUnitOfWork unitOfWork, 
		IMapper mapper)
    {
		_repositorio = repositorio;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

    public async Task Execute(long produtoId, RequestProdutoJson requisicao)
	{
		Validate(requisicao);

		var produto = await _repositorio.GetById(produtoId);

		if (produto is null)
		{
			throw new NotFoundException("Produto não encontrado.");
		}

		_mapper.Map(requisicao, produto);

		_repositorio.Atualize(produto);

		await _unitOfWork.Commit();
	}

	public static void Validate(RequestProdutoJson requisicao)
	{
		var validator = new ProdutoValidator();

		var resultado = validator.Validate(requisicao);

		if (resultado.IsValid == false)
		{
			var mensagensDeErro = resultado.Errors.Select(e => e.ErrorMessage).ToList();

			throw new ErrorOnValidationException(mensagensDeErro);
		}
	}
}
