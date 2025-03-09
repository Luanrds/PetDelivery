﻿using Aplicacao.Servicos;
using Aplicacao.UseCase.Carrinho.Atualizar;
using Aplicacao.UseCase.Carrinho.Buscar;
using Aplicacao.UseCase.Carrinho.Criar;
using Aplicacao.UseCase.Carrinho.Limpar;
using Aplicacao.UseCase.Carrinho.LimparCarrinho;
using Aplicacao.UseCase.Carrinho.RemoverItem;
using Aplicacao.UseCase.UseEndereco.Atualizar;
using Aplicacao.UseCase.UseEndereco.Buscar;
using Aplicacao.UseCase.UseEndereco.Criar;
using Aplicacao.UseCase.UseProduto.Atualizar;
using Aplicacao.UseCase.UseProduto.Criar;
using Aplicacao.UseCase.UseProduto.Excluir;
using Aplicacao.UseCase.UseProduto.GetById;
using Aplicacao.UseCase.UseProduto.ObtenhaTodosProdutos;
using Aplicacao.UseCase.UseUsuario.AlterarSenha;
using Aplicacao.UseCase.UseUsuario.Atualizar;
using Aplicacao.UseCase.UseUsuario.Buscar;
using Aplicacao.UseCase.UseUsuario.Criar;
using Aplicacao.UseCase.UseUsuario.Excluir;
using Microsoft.Extensions.DependencyInjection;

namespace Aplicacao;

public static class InjecaoDeDependenciaExtensao
{
	public static void AdicioneAplicacao(this IServiceCollection services)
	{
		AddAutoMapper(services);
		AdicioneUseCase(services);
	}

	private static void AddAutoMapper(IServiceCollection services)
	{
		services.AddScoped(opt => new AutoMapper.MapperConfiguration(options =>
		{
			options.AddProfile(new AutoMapping());
		}).CreateMapper());
	}

	private static void AdicioneUseCase(IServiceCollection services)
	{
		services.AddScoped<IUsuarioUseCase, UsuarioUseCase>();
		services.AddScoped<IObterUsuarioUseCase, ObterUsuarioUseCase>();
		services.AddScoped<IAtualizarUsuarioUseCase, AtualizarUsuarioUseCase>();
		services.AddScoped<IExcluirUsuarioUseCase, ExcluirUsuarioUseCase>();
		services.AddScoped<IAlterarSenhaUsuarioUseCase, AlterarSenhaUsuarioUseCase>();
		services.AddScoped<IEnderecoUseCase, EnderecoUseCase>();
		services.AddScoped<IBuscarEnderecosUseCase, BuscarEnderecosUseCase>();
		services.AddScoped<IAtualizeEnderecoUseCase, AtualizeEnderecoUseCase>();
		services.AddScoped<IProdutoUseCase, ProdutoUseCase>();
		services.AddScoped<IGetProdutoById, GetProdutoById>();
		services.AddScoped<IObtenhaTodosProdutos, ObtenhaTodosProdutos>();
		services.AddScoped<IExcluirProdutoUseCase, ExcluirProdutoUseCase>();
		services.AddScoped<IAtualizeProdutoUseCase, AtualizeProdutoUseCase>();
		services.AddScoped<ICarrinhoUseCase, CarrinhoUseCase>();
		services.AddScoped<IAtualizeQtdItemCarrinhoUseCase, AtualizeQtdItemCarrinhoUseCase>();
		services.AddScoped<IObterCarrinhoUseCase, ObterCarrinhoUseCase>();
		services.AddScoped<ILimpeCarrinhoUseCase, LimpeCarrinhoUseCase>();
		services.AddScoped<IRemoveItemCarrinhoUseCase, RemoveItemCarrinhoUseCase>();
	}
}