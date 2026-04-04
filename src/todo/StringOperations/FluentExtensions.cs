using System.Collections.Generic;

namespace Todo.StringOperations;

public static class FluentExtensions
{
    public static string StringJoin(this IEnumerable<string> stringSet, string delimiter) =>
        string.Join(delimiter, stringSet);
}