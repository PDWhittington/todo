using System;
using System.Diagnostics;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.Reporting;
using Todo.Git.Commands;

namespace Todo.Git;

public class GitInterface : IGitInterface
{
    private readonly IGitDependencyValidator _gitDependencyValidator;
    private readonly IGitExecutableResolver _gitExecutableResolver;
    private readonly IOutputFolderPathProvider _outputFolderPathProvider;
    private readonly IOutputWriter _outputWriter;

    public GitInterface(IGitDependencyValidator gitDependencyValidator,
        IGitExecutableResolver gitExecutableResolver,
        IOutputFolderPathProvider outputFolderPathProvider,
        IOutputWriter outputWriter)
    {
        _gitDependencyValidator = gitDependencyValidator;
        _gitExecutableResolver = gitExecutableResolver;
        _outputFolderPathProvider = outputFolderPathProvider;
        _outputWriter = outputWriter;
    }

    public bool RunGitCommand<T>(T command) where T : GitCommandBase
        => command.ExecuteCommand(this);

    public bool RunSpecialGitCommand(string command)
    {
        if (!_gitDependencyValidator.GitDependenciesValidated())
        {
            throw new Exception("Git not found at path given in settings file");
        }

        var commandWithGitSwitches = PrefixCommandWithGitSwitches(command);

        _outputWriter.WriteLine($"Executing git command: {commandWithGitSwitches}");

        var processStartInfo = new ProcessStartInfo(_gitExecutableResolver.GitPath, commandWithGitSwitches)
        {
            WorkingDirectory = _outputFolderPathProvider.GetRootedOutputFolder()
        };

        var process = Process.Start(processStartInfo) ?? throw new Exception("Process failed to start");

        process.WaitForExit();

        return true;
    }

    private string PrefixCommandWithGitSwitches(string command)
        => NoPager ? "--no-pager " + command : command;

    public bool NoPager { get; set; } = true;
}
