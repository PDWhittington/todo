using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;
using Todo.Git.Commands;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowConflictsCommandExecutor : CommandExecutorBase<ShowConflictsCommand>, IShowConflictsCommandExecutor
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IGitInterface _gitInterface;
    private readonly ITextFileLauncher _fileOpener;

    public ShowConflictsCommandExecutor(IOutputWriter outputWriter, IConfigurationProvider configurationProvider,
        IGitInterface gitInterface, ITextFileLauncher fileOpener) : base(outputWriter)
    {
        _configurationProvider = configurationProvider;
        _gitInterface = gitInterface;
        _fileOpener = fileOpener;
    }

    public override void Execute(ShowConflictsCommand command)
    {
        if (!_configurationProvider.ConfigInfo.Configuration.UseGit)
        {
            OutputWriter.WriteLine("This workspace is configured not to use git.");
            return;
        }

        var gitGetConflictsCommand = new GitGetConflictsCommand();
        var conflictCollection = _gitInterface
            .RunGitCommand<GitGetConflictsCommand, ConflictCollection>(gitGetConflictsCommand);

        if (!conflictCollection.Any())
        {
            OutputWriter.WriteLine("There are no conflicts in the current git");
            return;
        }

        var paths = conflictCollection.SelectMany(conflict =>
                new[]
                {
                    conflict.Ours.Path,
                    conflict.Theirs.Path
                })
            .Distinct()
            .Where(File.Exists)
            .ToArray();

        _fileOpener.LaunchFiles(paths);
    }
}
