using AutoMapper;
using Dominio.Repositorios.Produto;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.GetById;
public class GetProdutoById : IGetProdutoById
{
	private readonly IMapper _mapper;
	private readonly IProdutoReadOnly _repository;

	public GetProdutoById(IMapper mapper, IProdutoReadOnly repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<ResponseProdutoJson> ExecuteAsync(long ProdutoId)
	{
		var produto = await _repository.GetById(ProdutoId);

		if (produto is null)
		{
			throw new NotFoundException("Produto não encontrado.");
		}
	
		var response = _mapper.Map<ResponseProdutoJson>(produto);

		return response;
	}
}
