using Dominio.Interfaces.Generics;

namespace Infrastucture.Repositorio.Generico;

public class RepositorioGenerics<T> : IGenerics<T>, IDisposable where T : class
{
    public Task Add(T Objeto)
    {
        throw new NotImplementedException();
    }

    public Task Delete(T Objeto)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public Task<T> GetEntityById(int Id)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> List()
    {
        throw new NotImplementedException();
    }

    public Task Update(T Objeto)
    {
        throw new NotImplementedException();
    }
}
