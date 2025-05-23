using Microsoft.AspNetCore.Http;

namespace PetDelivery.Communication.Request;
public class RequestRegistroProdutoFormData : RequestProdutoJson
{
	public IFormFile? Imagem { get; set; }
}
