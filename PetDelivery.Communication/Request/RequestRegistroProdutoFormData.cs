using Microsoft.AspNetCore.Http;

namespace PetDelivery.Communication.Request;
public class RequestRegistroProdutoFormData : RequestProdutoJson
{
	public List<IFormFile>? Imagens { get; set; }
}
