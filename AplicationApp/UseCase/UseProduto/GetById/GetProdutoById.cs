using AutoMapper;
using Dominio.Repositorios.Produto;
using PetDelivery.Communication.Response;

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

	public async Task<ResponseProdutoJson> Execute(long ProdutoId)
	{
		var produto = await _repository.GetById(ProdutoId);

		if(produto is not null)
		{
			throw new Exception("Não");
		}

		var response = _mapper.Map<ResponseProdutoJson>(produto);

		return response;
	}
}
