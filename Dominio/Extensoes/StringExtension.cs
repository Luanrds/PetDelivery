using System.Diagnostics.CodeAnalysis;

namespace Dominio.Extensoes;
public static class StringExtension
{
	public static bool NotEmpty([NotNullWhen(true)] this string? value) => string.IsNullOrWhiteSpace(value).IsFalse();
}
