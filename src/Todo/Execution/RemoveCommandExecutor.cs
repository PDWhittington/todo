using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

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
        if (!_dateListPathResolver.TryResolvePathFor(command.Date, FileTypeEnum.MarkdownDayList, out var pathForFile))
        {
            OutputWriter.WriteLine($"Could not find file for {command.Date}");
            return;
        }

        if (_configurationProvider.ConfigInfo.Configuration.UseGit)
        {
            var gitRemoveCommand = new GitRemoveCommand(pathForFile!.Path);
            var gitResult = _gitInterface.RunGitCommand<GitRemoveCommand, VoidResult>(gitRemoveCommand);
            if (!gitResult.Success) throw gitResult.Exception
                                          ?? new Exception("Some exception in git rm command");
        }
        else
        {
            File.Delete(pathForFile!.Path);
        }
    }
}
