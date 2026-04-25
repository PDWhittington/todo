using System.Drawing;
using System.Text.Json.Serialization;
using Todo.StateAndConfig;

namespace Todo.Contracts.Data.Config;

[method: JsonConstructor]
public record ScoreCategory(string Name, string MarkdownHeading, 
    [property: JsonConverter(typeof(ColorFormatter))]
    Color GraphColor, bool IsDefaultCategory);