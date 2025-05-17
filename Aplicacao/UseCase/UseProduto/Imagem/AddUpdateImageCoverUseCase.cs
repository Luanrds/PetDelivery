using Dominio.Entidades;
using Dominio.Repositorios;
using Dominio.Repositorios.Produto;
using Dominio.Servicos.UsuarioLogado;
using Microsoft.AspNetCore.Http;
using PetDelivery.Exceptions.ExceptionsBase;

namespace Aplicacao.UseCase.UseProduto.Imagem;
public class AddUpdateImageCoverUseCase : IAddUpdateImageCoverUseCase
{
	private readonly IUsuarioLogado _usuarioLogado;
	private readonly IProdutoUpdateOnly _produtoUpdateOnly;
	private readonly IUnitOfWork _unitOfWork;

    public AddUpdateImageCoverUseCase(
		IUsuarioLogado usuarioLogado, 
		IProdutoUpdateOnly produtoUpdateOnly, 
		IUnitOfWork unitOfWork)
    {
		_produtoUpdateOnly = produtoUpdateOnly;
		_unitOfWork = unitOfWork;
		_usuarioLogado = usuarioLogado;
	}

    public async Task Execute(long produtoId, IFormFile file)
	{
		Usuario usuarioLogado = await _usuarioLogado.Usuario();

		Produto? produto = await _produtoUpdateOnly.GetById(produtoId)
			?? throw new InvalidOperationException($"Produto não encontrado.");



		throw new NotImplementedException();
	}
}
