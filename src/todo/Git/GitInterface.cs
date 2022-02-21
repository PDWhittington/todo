using System;
using System.Diagnostics;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Git;

public class GitInterface : IGitInterface
{
    private readonly IGitDependencyValidator _gitDependencyValidator;
    private readonly IGitExecutableResolver _gitExecutableResolver;
    private readonly IConfigurationProvider _configurationProvider;

    public GitInterface(IGitDependencyValidator gitDependencyValidator,
        IGitExecutableResolver gitExecutableResolver,
        IConfigurationProvider configurationProvider)
    {
        _gitDependencyValidator = gitDependencyValidator;
        _gitExecutableResolver = gitExecutableResolver;
        _configurationProvider = configurationProvider;
    }

    public bool RunGitCommand(string command)
    {
        if (!_gitDependencyValidator.GitDependenciesValidated())
        {
            throw new Exception("Git not found at path given in settings file");
        }

        var processStartInfo = new ProcessStartInfo(_gitExecutableResolver.GitPath, command)
        {
            WorkingDirectory = _configurationProvider.Config.OutputFolder
        };

        var process = Process.Start(processStartInfo) ?? throw new Exception("Process failed to start");

        process.WaitForExit();

        return true;
    }
}
