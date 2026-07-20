using System.Text.Json.Serialization;

namespace Todo.Contracts.Data.Config;

[method: JsonConstructor]
public record PerOsLaunchInfos(
    ProcessLaunchInfo Windows,
    ProcessLaunchInfo Linux,
    // ReSharper disable once InconsistentNaming
    ProcessLaunchInfo OSX,
    string? EnvironmentVariableToOverridePath = null,
    string? EnvironmentVariableToOverrideParameters = null);
