using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;
using Todo.Git.Commands;
using Todo.Git.Results;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class RemoveCommandExecutor : CommandExecutorBase<RemoveCommand>, IRemoveCommandExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IDateListPathResolver _dateListPathResolver;
    private readonly IGitInterface _gitInterface;

    public RemoveCommandExecutor(IOutputWriter outputWriter, IConfigurationProvider configurationProvider,
        IDateListPathResolver dateListPathResolver, IGitInterface gitInterface)
        : base(outputWriter)
    {
        _configurationProvider = configurationProvider;
        _dateListPathResolver = dateListPathResolver;
        _gitInterface = gitInterface;
    }

    public override void Execute(RemoveCommand command)
    {
        var pathForFile = _dateListPathResolver.ResolvePathFor(
            command.Date, FileTypeEnum.Markdown, false);

        if (_configurationProvider.Config.UseGit)
        {
            var gitRemoveCommand = new GitRemoveCommand(pathForFile.Path);
            var gitResult = _gitInterface.RunGitCommand<GitRemoveCommand, VoidResult>(gitRemoveCommand);
            if (!gitResult.Success) throw gitResult.Exception
                                          ?? new Exception("Some exception in git rm command");
        }
        else
        {
            File.Delete(pathForFile.Path);
        }
    }
}
