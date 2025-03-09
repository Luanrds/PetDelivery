namespace Dominio.Repositorios.Endereco;
public interface IEnderecoWriteOnly
{
	Task Add(Entidades.Endereco endereco);
	void Atualize(Entidades.Endereco produto);
	void Excluir(Entidades.Endereco endereco);
}
