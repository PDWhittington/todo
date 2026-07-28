using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;
using Todo.Git.Branches;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PushCommandExecutor(
    IConfigurationProvider configurationProvider,
    IGitInterface gitInterface,
    IOutputWriter outputWriter,
    ILogger<PushCommandExecutor> logger)
    : CommandExecutorBase<PushCommand>(outputWriter, logger), IPushCommandExecutor
{
    public override void Execute(PushCommand command)
    {
        if (!configurationProvider.ConfigInfo.Configuration.UseGit)
        {
            OutputWriter.WriteLine(
                "Pushing does not make sense when UseGit is set to false in the settings file.");
            return;
        }

        var result = gitInterface.RunGitCommand<GitPushCommand, VoidResult>(
            new GitPushCommand(HeadBranchLocator.Instance));

        //We need a better error handling scheme all-round.
        if (!result.Success)
            throw result.Exception ?? new Exception("Some issue with push command");
    }
}