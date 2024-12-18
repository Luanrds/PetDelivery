using AutoMapper;
using PetDelivery.Communication.Request;

namespace Aplicacao.Servicos;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
    }

    private void RequestToDomain()
    {
        CreateMap<RequestProdutoJson, Dominio.Entidades.Produto>();
    }
}
