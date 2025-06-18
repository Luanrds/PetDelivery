using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Pagamento;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.Pagamento.Excluir;
public class ExcluirMetodoPagamentoUseCase : IExcluirMetodoPagamentoUseCase
{
	private readonly IMetodoPagamentoUsuarioRepository _repository;
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IUnitOfWork _unitOfWork;

	public ExcluirMetodoPagamentoUseCase(IMetodoPagamentoUsuarioRepository repository, IUsuarioLogado usuarioLogado, IUnitOfWork unitOfWork)
	{
		_repository = repository;
		_usuarioLogado = usuarioLogado;
		_unitOfWork = unitOfWork;
	}

	public async Task Execute(long id)
	{
		Usuario usuario = await _usuarioLogado.Usuario();

		MetodoPagamentoUsuario? metodoPagamento = await _repository.ObterPorIdEUsuarioId(id, usuario.Id) 
			?? throw new NotFoundException("Método de pagamento não encontrado.");

		_repository.Excluir(metodoPagamento);

		await _unitOfWork.Commit();
	}
}