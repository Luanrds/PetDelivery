﻿namespace Dominio.Interfaces.Generics;
public interface IGenerics<T> where T : class
{
	Task Add(T Objeto);

	Task Update(T Objeto);

	Task Delete(T Objeto);

	Task<T> GetEntityById(int Id);

	Task<List<T>> List();
}
