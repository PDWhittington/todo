using System.Drawing;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Todo.Contracts.Data.Config;

public class ColorFormatter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Handle the most common cases from your old Utf8Json formatter
        if (reader.TokenType != JsonTokenType.String) throw new JsonException();
        
        var colorText = reader.GetString();
        
        if (string.IsNullOrWhiteSpace(colorText))
            return Color.Empty; // or default(Color)

        // Support named colors, hex (#RRGGBB or #RRGGBBAA), etc.
        return colorText.StartsWith('#')
            ? ColorTranslator.FromHtml(colorText)  // works for #RGB, #RRGGBB, #RRGGBBAA
            : Color.FromName(colorText); // Named color (e.g. "Red", "AliceBlue")
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        // Decide on your preferred output format (hex is common and compact)
        if (value.IsNamedColor && !string.IsNullOrEmpty(value.Name))
        {
            writer.WriteStringValue(value.Name);
        }
        else if (value.A == 255)
        {
            writer.WriteStringValue($"#{value.R:X2}{value.G:X2}{value.B:X2}");
        }
        else
        {
            writer.WriteStringValue($"#{value.R:X2}{value.G:X2}{value.B:X2}{value.A:X2}");
        }
    }
}