using Dominio.Interfaces.InterfaceProducts;
using Entidades.Entidades;
using Infrastucture.Repositorio.Generico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repositorio.Repositorios
{
    public class RepositoryProduct : RepositoryGenerics<Produto>, IProduct
    {
    }
}
