using System.Diagnostics;
using Todo.Contracts.Services;

namespace Todo.Git;

public class GitInterface : IGitInterface
{
    private readonly IConfigurationProvider _configurationProvider;

    public GitInterface(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }
    
    public bool RunGitCommand(string command)
    {
        var configuration = _configurationProvider.GetConfiguration();

        var processStartInfo = new ProcessStartInfo(configuration.GitPath, command)
        {
            WorkingDirectory = configuration.OutputFolder
        };

        var process = Process.Start(processStartInfo);

        process.WaitForExit();

        return true;
    }
}