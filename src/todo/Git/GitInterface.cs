using System;
using System.Diagnostics;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Git;

public class GitInterface : IGitInterface
{
    private readonly IGitDependencyValidator _gitDependencyValidator;
    private readonly IConfigurationProvider _configurationProvider;

    public GitInterface(IGitDependencyValidator gitDependencyValidator, IConfigurationProvider configurationProvider)
    {
        _gitDependencyValidator = gitDependencyValidator;
        _configurationProvider = configurationProvider;
    }
    
    public bool RunGitCommand(string command)
    {
        if (!_gitDependencyValidator.GitDependenciesValidated())
        {
            throw new Exception("Git not found at path given in settings file");
        }
        
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