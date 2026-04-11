using System.Text.Json.Serialization;

namespace Todo.Contracts.Data.Config;

[method: JsonConstructor]
public record ProcessLaunchInfo(string Path, string Parameters)
{
    // ReSharper disable once MemberCanBePrivate.Global

    public string InterpolateParameters(string filePath)
        => string.Format(Parameters, filePath);
}
