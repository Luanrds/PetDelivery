using AutoMapper;
using Dominio.Entidades;
using PetDelivery.Communication.Request;
using PetDelivery.Communication.Response;
using Sqids;

namespace Aplicacao.Servicos;

public class AutoMapping : Profile
{
    private readonly SqidsEncoder<long> _idEncoder;

    public AutoMapping(SqidsEncoder<long> idEncoder)
    {
        _idEncoder = idEncoder;

		RequestToDomain();
        DomainToResponse();
	}

    private void RequestToDomain()
    {
        CreateMap<RequestProdutoJson, Produto>();
    }

    private void DomainToResponse()
    {
		CreateMap<Produto, ResponseProdutoJson>()
	        .ForMember(dest => dest.Id, config => config.MapFrom(source => _idEncoder.Encode(source.Id)));
	}
}
