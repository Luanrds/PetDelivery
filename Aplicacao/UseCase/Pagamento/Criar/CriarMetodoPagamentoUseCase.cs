using Aplicacao.UseCase.Pagamento.Criar;
using AutoMapper;
using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Pagamento;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.UseCase.Pagamento.CriarMetodoPagamento;
public class CriarMetodoPagamentoUseCase : ICriarMetodoPagamentoUseCase
{
    private readonly IMetodoPagamentoUsuarioRepository _repository;
    private readonly IUsuarioLogado _usuarioLogado;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CriarMetodoPagamentoUseCase(IMetodoPagamentoUsuarioRepository repository, IUsuarioLogado usuarioLogado, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _usuarioLogado = usuarioLogado;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResponseCartaoCreditoJson> Execute(RequestCartaoCreditoJson request)
    {
        Usuario usuario = await _usuarioLogado.Usuario();

        MetodoPagamentoUsuario metodoPagamento = _mapper.Map<MetodoPagamentoUsuario>(request);
        metodoPagamento.UsuarioId = usuario.Id;

        await _repository.Adicionar(metodoPagamento);
        await _unitOfWork.Commit();

        return _mapper.Map<ResponseCartaoCreditoJson>(metodoPagamento);
    }
}
