using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Git.Commands;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PushCommandExecutor : CommandExecutorBase<PushCommand>, IPushCommandExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IGitInterface _gitInterface;

    public PushCommandExecutor(IConfigurationProvider configurationProvider, IGitInterface gitInterface)
    {
        _configurationProvider = configurationProvider;
        _gitInterface = gitInterface;
    }

    public override void Execute(PushCommand command)
    {
        if (!_configurationProvider.Config.UseGit)
            throw new Exception("Pushing does not make sense when UseGit is set to false in the settings file.");

        _gitInterface.RunGitCommand(new GitPushCommand());
    }
}
