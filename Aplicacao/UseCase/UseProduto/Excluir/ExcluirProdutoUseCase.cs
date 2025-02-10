using AutoMapper;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.Excluir;
public class ExcluirProdutoUseCase : IExcluirProdutoUseCase
{
	private readonly IMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IProdutoReadOnly _repositoryRead;
	private readonly IProdutoWriteOnly _repositoryWrite;


	public ExcluirProdutoUseCase(
		IMapper mapper, 
		IProdutoReadOnly repositoryRead, 
		IProdutoWriteOnly repositoryWrite, 
		IUnitOfWork unitOfWork)
	{
		_mapper = mapper;
		_repositoryRead = repositoryRead;
		_repositoryWrite = repositoryWrite;
		_unitOfWork = unitOfWork;
	}

	public async Task Execute(long produtoId)
	{
		var produto = await _repositoryRead.GetById(produtoId);

		if (produto is null)
		{
			throw new NotFoundException("Produto não encontrado.");
		}

		await _repositoryWrite.Excluir(produtoId);

		await _unitOfWork.Commit();
	}
}
