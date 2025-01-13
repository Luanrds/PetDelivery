using AutoMapper;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;

namespace Aplicacao.Servicos;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToDomain();
        DomainToResponse();

	}

    private void RequestToDomain()
    {
        CreateMap<RequestProdutoJson, Dominio.Entidades.Produto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
	}

    private void DomainToResponse()
    {
        CreateMap<Dominio.Entidades.Produto, ResponseProdutoJson>()
            .ForMember(dest => dest.Id, config => config.MapFrom(source => source.Id));
    }
}
