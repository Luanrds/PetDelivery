using Aplicacao.UseCase.Pagamento.Listar;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios.Pagamento;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Pagamento.ListarMetodoPagamento;

public class ListarMetodosPagamentoUseCase : IListarMetodosPagamentoUseCase
{
	private readonly IMetodoPagamentoUsuarioRepository _repository;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IMapper _mapper;

	public ListarMetodosPagamentoUseCase(IMetodoPagamentoUsuarioRepository repository, IUsuarioLogado usuarioLogado, IMapper mapper)
	{
		_repository = repository;
		_usuarioLogado = usuarioLogado;
		_mapper = mapper;
	}

	public async Task<IList<ResponseCartaoCreditoJson>> Execute()
	{
		Usuario usuario = await _usuarioLogado.Usuario();

		IList<MetodoPagamentoUsuario> metodosPagamento = 
			await _repository.ObterPorUsuarioId(usuario.Id);

		return _mapper.Map<IList<ResponseCartaoCreditoJson>>(metodosPagamento);
	}
}