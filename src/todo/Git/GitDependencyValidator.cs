using System.IO;
using Todo.Contracts.Services;

namespace Todo.Git;

public class GitDependencyValidator : IGitDependencyValidator
{
    private readonly IConfigurationProvider _configurationProvider;

    public GitDependencyValidator(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }
    
    public bool IsGitPresent()
    {
        var configuration = _configurationProvider.GetConfiguration();
        return File.Exists(configuration.GitPath);
    }
}