using FileTypeChecker.Extensions;
using FileTypeChecker.Types;

namespace Aplicacao.Extensoes;
public static class StreamImageExtensions
{
	public static (bool isValidImage, string extension) ValidateAndGetImageExtension(this Stream stream)
	{
		var resultado = (false, string.Empty);

		if (stream.Is<PortableNetworkGraphic>())
		{
			resultado = (true, NormalizeExtensao(PortableNetworkGraphic.TypeExtension));
		}
		else if (stream.Is<JointPhotographicExpertsGroup>())
		{
			resultado = (true, NormalizeExtensao(JointPhotographicExpertsGroup.TypeExtension));
		}
		stream.Position = 0;

		return resultado;
	}

	private static string NormalizeExtensao(string extension)
	{
		return extension.StartsWith('.') ? extension : $".{extension}";
	}
}
