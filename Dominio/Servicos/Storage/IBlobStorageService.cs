using Dominio.Entidades;

namespace Dominio.Servicos.Storage;
public interface IBlobStorageService
{
	Task Uploud(Usuario usuario, Stream file, string fileName);
	Task<string> GetFileUrl(Usuario usuario, string fileName);
	Task Excluir(Usuario usuario, string fileName);
}
