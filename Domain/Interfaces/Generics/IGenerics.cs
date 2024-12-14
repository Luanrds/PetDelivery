namespace Dominio.Interfaces.Generics;
public interface IGenerics<T> where T : class
{
	Task<bool> Add(T Objeto);

	Task<bool> Update(T Objeto);

	Task<bool> Delete(T Objeto);

	Task<T> GetEntityById(int Id);

	Task<List<T>> List();
}
