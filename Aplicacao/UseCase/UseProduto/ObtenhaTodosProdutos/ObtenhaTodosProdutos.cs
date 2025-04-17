using AutoMapper;
using Dominio.Repositorios.Produto;
using PetDelivery.Communication.Response;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.ObtenhaTodosProdutos;
public class ObtenhaTodosProdutos : IObtenhaTodosProdutos
{
	private readonly IMapper _mapper;
	private readonly IProdutoReadOnly _repository;

	public ObtenhaTodosProdutos(IMapper mapper, IProdutoReadOnly repository)
	{
		_mapper = mapper;
		_repository = repository;
	}

	public async Task<IEnumerable<ResponseProdutoJson>> ExecuteAsync()
	{
		var produtos = await _repository.GetAll();

		if (produtos.Count == 0)
		{
			throw new NotFoundException("Nenhum produto encontrado.");
		}

		return _mapper.Map<IEnumerable<ResponseProdutoJson>>(produtos);
	}
}
