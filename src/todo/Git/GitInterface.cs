using System;
using System.Diagnostics;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Git;

public class GitInterface : IGitInterface
{
    private readonly IGitDependencyValidator _gitDependencyValidator;
    private readonly IGitExecutableResolver _gitExecutableResolver;
    private readonly IOutputFolderPathProvider _outputFolderPathProvider;

    public GitInterface(IGitDependencyValidator gitDependencyValidator,
        IGitExecutableResolver gitExecutableResolver,
        IOutputFolderPathProvider outputFolderPathProvider)
    {
        _gitDependencyValidator = gitDependencyValidator;
        _gitExecutableResolver = gitExecutableResolver;
        _outputFolderPathProvider = outputFolderPathProvider;
    }

    public bool RunGitCommand(string command)
    {
        if (!_gitDependencyValidator.GitDependenciesValidated())
        {
            throw new Exception("Git not found at path given in settings file");
        }

        var processStartInfo = new ProcessStartInfo(_gitExecutableResolver.GitPath, command)
        {
            WorkingDirectory = _outputFolderPathProvider.GetRootedOutputFolder()
        };

        var process = Process.Start(processStartInfo) ?? throw new Exception("Process failed to start");

        process.WaitForExit();

        return true;
    }
}
