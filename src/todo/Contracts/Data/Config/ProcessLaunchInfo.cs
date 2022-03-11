using System.Text.Json.Serialization;

namespace Todo.Contracts.Data.Config;

public class ProcessLaunchInfo
{
    public string Path { get; }

    // ReSharper disable once MemberCanBePrivate.Global
    public string Parameters { get; }

    [JsonConstructor]
    public ProcessLaunchInfo(string path, string parameters)
    {
        Path = path;
        Parameters = parameters;
    }

    public string InterpolateParameters(string filePath)
        => string.Format(Parameters, filePath);
}
