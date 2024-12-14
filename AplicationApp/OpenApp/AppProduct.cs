using Aplicacao.Interfaces;
using Dominio.Interfaces.InterfaceProduct;
using Dominio.Interfaces.InterfaceServices;
using Entidades.Entidades;

namespace Aplicacao.OpenApp;

public class AppProduct(IProduct IProduct, IServiceProduct IServiceProduct) : InterfaceProductApp
{
	private readonly IProduct _IProduct = IProduct;

	private readonly IServiceProduct _IServiceProduct = IServiceProduct;

	public async Task AddProduct(Produto produto)
	{
		await _IServiceProduct.AddProduct(produto);
	}

	public async Task UpdateProduct(Produto produto)
	{
		await _IServiceProduct.UpdateProduct(produto);
	}

	public async Task Add(Produto Objeto)
	{
		await _IProduct.Add(Objeto);
	}

	public async Task Delete(Produto Objeto)
	{
		await _IProduct.Delete(Objeto);
	}

	public async Task<Produto> GetEntityById(int Id)
	{
		return await _IProduct.GetEntityById(Id);
	}

	public async Task<List<Produto>> List()
	{
		return await _IProduct.List();
	}

	public async Task Update(Produto Objeto)
	{
		await _IProduct.Update(Objeto);
	}
}
