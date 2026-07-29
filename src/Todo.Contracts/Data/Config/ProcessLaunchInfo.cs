using System.Text.Json.Serialization;

namespace Todo.Contracts.Data.Config;

[method: JsonConstructor]
public record ProcessLaunchInfo(string Path, string Arguments)
{
    public string InterpolateParameters(string filePath)
        => string.Format(Arguments, filePath);
}
