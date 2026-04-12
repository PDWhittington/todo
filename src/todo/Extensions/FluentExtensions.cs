using System.Collections.Generic;
using System.Drawing;

namespace Todo.Extensions;

public static class FluentExtensions
{
    public static string StringJoin(this IEnumerable<string> stringSet, string delimiter) =>
        string.Join(delimiter, stringSet);
    
    public static string ToHex(this Color color)
        => $"#{color.R:X2}{color.G:X2}{color.B:X2}";
}