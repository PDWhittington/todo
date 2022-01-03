using System.IO;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Git;

public class GitDependencyValidator : IGitDependencyValidator
{
    private readonly IConfigurationProvider _configurationProvider;

    private bool? _validated;

    public GitDependencyValidator(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }

    public bool GitDependenciesValidated()
    {
        _validated ??= File.Exists(_configurationProvider.Config.GitPath);

        return (bool)_validated;
    }
}
