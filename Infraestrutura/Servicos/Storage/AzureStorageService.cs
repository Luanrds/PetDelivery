using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Dominio.Entidades;
using Dominio.Extensoes;
using Dominio.Servicos.Storage;

namespace Infraestrutura.Servicos.Storage;
public class AzureStorageService : IBlobStorageService
{
	private readonly BlobServiceClient _blobServiceClient;
	private const int MAXIMO_TEMPO_DE_VIDA_URL_IMAGEM = 10;

	public AzureStorageService(BlobServiceClient blobServiceClient)
	{
		_blobServiceClient = blobServiceClient;
	}

	public async Task Uploud(Usuario usuario, Stream file, string fileName)
	{
		var container = _blobServiceClient.GetBlobContainerClient(usuario.IdentificadorDoUsuario.ToString());
		await container.CreateIfNotExistsAsync();

		var blobClient = container.GetBlobClient(fileName);

		await blobClient.UploadAsync(file, overwrite: true);
	}

	public async Task<string> GetFileUrl(Usuario usuario, string fileName)
	{
		string nomeContainer = usuario.IdentificadorDoUsuario.ToString();

		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(nomeContainer);
		Azure.Response<bool> existe = await containerClient.ExistsAsync();

		if (existe.Value.IsFalse())
		{
			return string.Empty;
		}

		BlobClient blobClient = containerClient.GetBlobClient(fileName);

		existe = await blobClient.ExistsAsync();

		if (existe.Value)
		{
			var sasBuilder = new BlobSasBuilder
			{
				BlobContainerName = nomeContainer,
				BlobName = fileName,
				Resource = "b",
				ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(MAXIMO_TEMPO_DE_VIDA_URL_IMAGEM),
			};

			sasBuilder.SetPermissions(BlobSasPermissions.Read);

			return blobClient.GenerateSasUri(sasBuilder).ToString();
		}

		return string.Empty;
	}

	public async Task Excluir(Usuario usuario, string fileName)
	{
		BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(usuario.IdentificadorDoUsuario.ToString());
		Azure.Response<bool> existe = await containerClient.ExistsAsync();

		if (existe.Value)
		{
			await containerClient.DeleteBlobIfExistsAsync(fileName);
		}
	}
}
