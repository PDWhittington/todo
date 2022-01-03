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

        var processStartInfo = new ProcessStartInfo(_configurationProvider.Config.GitPath, command)
        {
            WorkingDirectory = _configurationProvider.Config.OutputFolder
        };

        var process = Process.Start(processStartInfo);

        process.WaitForExit();

        return true;
    }
}