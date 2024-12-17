using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace PetDelivery.API.Conversoes;

public partial class ConvertaString : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var valor = reader.GetString()?.Trim();

        if (valor == null)
        {
            return null;
        }

        return RemoveEspacosVazios().Replace(valor, " ");        
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options) => writer.WriteStringValue(value);

    [GeneratedRegex(@"\s+")]
    private static partial Regex RemoveEspacosVazios();
}
