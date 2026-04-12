using System.Drawing;
using System.Text.Json.Serialization;

namespace Todo.Contracts.Data.Config;

[method: JsonConstructor]
public record ScoreCategory(string Name, string MarkdownHeading, Color GraphColor);