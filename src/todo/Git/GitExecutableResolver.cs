using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Git;

public class GitExecutableResolver : IGitExecutableResolver
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IPathHelper _pathHelper;
    private string? _gitPath;

    public GitExecutableResolver(IConfigurationProvider configurationProvider,
        IPathHelper pathHelper)
    {
        _configurationProvider = configurationProvider;
        _pathHelper = pathHelper;
    }

    public string GitPath => _gitPath ?? GetGitPath();

    private string GetGitPath()
    {
        _gitPath = _pathHelper.ResolveIfNotRooted(_configurationProvider.Config.GitPath.GetPathForThisOs().Path);
        return _gitPath;
    }
}
