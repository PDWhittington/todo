using System;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Execution;

public class PushExecutor : IPushExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IGitInterface _gitInterface;

    public PushExecutor(IConfigurationProvider configurationProvider, IGitInterface gitInterface)
    {
        _configurationProvider = configurationProvider;
        _gitInterface = gitInterface;
    }

    public void Execute(PushCommand command)
    {
        if (!_configurationProvider.Config.UseGit)
            throw new Exception("Pushing does not make sense when UseGit is set to false in the settings file.");

        _gitInterface.RunGitCommand("push");
    }
}
