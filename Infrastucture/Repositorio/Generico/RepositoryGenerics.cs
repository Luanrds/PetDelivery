using Dominio.Interfaces.Generics;
using Infrastucture.Configuracao;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Infrastucture.Repositorio.Generico;

public class RepositoryGenerics<T> : IGenerics<T>, IDisposable where T : class
{
    private readonly ContextBase _context;

    public RepositoryGenerics(ContextBase context)
    {
        _context = context;
    }

    public async Task Add(T Objeto)
    {
        _context.Set<T>().Add(Objeto);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(T Objeto)
    {
        _context.Set<T>().Remove(Objeto);
        await _context.SaveChangesAsync();
    }

    public async Task<T> GetEntityById(int Id)
    {
        return await _context.Set<T>().FindAsync(Id);
    }

    public async Task<List<T>> List()
    {
        return await _context.Set<T>().AsNoTracking().ToListAsync();
    }

    public async Task Update(T Objeto)
    {
        _context.Set<T>().Update(Objeto);
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}
