using System.Drawing;
using Utf8Json;

namespace Todo.StateAndConfig;

public class ColorFormatter : IJsonFormatter<Color>
{
    public void Serialize(ref JsonWriter writer, Color value, IJsonFormatterResolver formatterResolver)
    {
        if (value.IsNamedColor && value.IsKnownColor)
        {
            // Output the known color name (e.g. "Red", "AliceBlue")
            writer.WriteString(value.Name);
        }
        else
        {
            // Output as #RRGGBB (24-bit hex, ignoring alpha for simplicity)
            // You could include alpha as #AARRGGBB if needed
            writer.WriteString($"#{value.R:X2}{value.G:X2}{value.B:X2}");
        }
    }

    public Color Deserialize(ref JsonReader reader, IJsonFormatterResolver formatterResolver)
    {
        var token = reader.GetCurrentJsonToken();

        if (token == JsonToken.Null)
        {
            reader.ReadIsNull();
            return Color.Empty; // or Color.Transparent, depending on your needs
        }

        if (token == JsonToken.String)
        {
            var str = reader.ReadString();

            // Try parsing as named color first (case-insensitive)
            var namedColor = Color.FromName(str);
            if (namedColor.IsKnownColor)
                return namedColor;

            // Fall back to hex (#RRGGBB or RRGGBB)
            if (str.StartsWith("#"))
                str = str.Substring(1);

            if (str.Length == 6 && int.TryParse(str, System.Globalization.NumberStyles.HexNumber, null, out int rgb))
            {
                return Color.FromArgb(
                    (byte)((rgb >> 16) & 0xFF),  // R
                    (byte)((rgb >> 8) & 0xFF),   // G
                    (byte)(rgb & 0xFF)           // B
                );
            }

            // Optional: support #RGB shorthand, #AARRGGBB, etc.
            throw new JsonParsingException($"Cannot parse color from string: {str}");
        }

        // Fallback: if someone passes an object or number, you can extend here
        throw new JsonParsingException($"Unexpected token for Color: {token}");
    }
}