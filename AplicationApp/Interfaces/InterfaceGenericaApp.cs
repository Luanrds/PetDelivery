﻿namespace Aplicacao.Interfaces
{
    public interface InterfaceGenericaApp<T> where T : class
    {
        Task Add(T Objeto);

        Task Update(T Objeto);

        Task Delete(T Objeto);

        Task<T> GetEntityById(int Id);

        Task<List<T>> List();
    }
}
