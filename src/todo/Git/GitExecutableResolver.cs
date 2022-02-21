using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Git;

public class GitExecutableResolver : IGitExecutableResolver
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IEnvironmentPathResolver _environmentPathResolver;
    private string? _gitPath;

    public GitExecutableResolver(IConfigurationProvider configurationProvider,
        IEnvironmentPathResolver environmentPathResolver)
    {
        _configurationProvider = configurationProvider;
        _environmentPathResolver = environmentPathResolver;
    }

    public string GitPath => _gitPath ?? GetGitPath();

    private string GetGitPath()
    {
        _gitPath = _environmentPathResolver.ResolveIfNotRooted(_configurationProvider.Config.GitPath.GetPathForThisOs());
        return _gitPath;
    }
}
