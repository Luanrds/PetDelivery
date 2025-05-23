using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Endereco;
using Dominio.Servicos.UsuarioLogado;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseEndereco.Excluir;
public class ExcluirEnderecoUseCase : IExcluirEnderecoUseCase
{
	private readonly IEnderecoReadOnly _enderecoReadOnly;
	private readonly IEnderecoWriteOnly _enderecoWriteOnly;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IUsuarioLogado _usuarioLogado;

	public ExcluirEnderecoUseCase(
		IEnderecoReadOnly enderecoReadOnly,
		IEnderecoWriteOnly enderecoWriteOnly,
		IUnitOfWork unitOfWork,
		IUsuarioLogado usuarioLogado)
	{
		_enderecoReadOnly = enderecoReadOnly;
		_enderecoWriteOnly = enderecoWriteOnly;
		_unitOfWork = unitOfWork;
		_usuarioLogado = usuarioLogado;
	}

	public async Task ExecuteAsync(long enderecoId)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		Endereco? endereco = await _enderecoReadOnly.GetById(usuarioLogado.Id, enderecoId);

		if (endereco is null || endereco.UsuarioId != usuarioLogado.Id)
		{
			throw new NotFoundException($"Endereço com ID {enderecoId} não encontrado ou não pertence a este usuário.");
		}

		_enderecoWriteOnly.Excluir(endereco);


		await _unitOfWork.Commit();
	}
}
