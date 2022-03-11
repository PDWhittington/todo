using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;
using Todo.Git.Commands;
using Todo.Git.Results;

namespace Todo.Execution;

public abstract class CreateOrShowCommandExecutorBase<TCommandType, TSubstitutionsType> : CommandExecutorBase<TCommandType>
    where TCommandType : CreateOrShowCommandBase
    where TSubstitutionsType : MarkdownSubstitutionsBase
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IGitInterface _gitInterface;
    private readonly ITextFileLauncher _fileOpener;

    protected CreateOrShowCommandExecutorBase(IConfigurationProvider configurationProvider,
        IGitInterface gitInterface, ITextFileLauncher fileOpener, IOutputWriter outputWriter)
        : base(outputWriter)
    {
        _configurationProvider = configurationProvider;
        _gitInterface = gitInterface;
        _fileOpener = fileOpener;
    }

    public override void Execute(TCommandType createOrShowCommand)
    {
        var pathInfo = GetFilePathInfo(createOrShowCommand);

        if (!File.Exists(pathInfo.Path))
        {
            var templateFile = GetTemplate();

            var markdownSubstitutions = GetMarkdownSubstitutions(createOrShowCommand);

            var outputText = MakeSubstitutions(markdownSubstitutions, templateFile.FileContents);

            File.WriteAllText(pathInfo.Path, outputText);

            if (_configurationProvider.Config.UseGit)
            {
                _gitInterface.RunGitCommand<GitAddCommand, VoidResult>(new GitAddCommand(pathInfo.Path));
            }
        }

        _fileOpener.LaunchFiles(pathInfo.Path);
    }

    protected abstract FilePathInfo GetFilePathInfo(TCommandType createOrShowCommand);

    protected abstract TodoFile GetTemplate();

    protected abstract TSubstitutionsType GetMarkdownSubstitutions(TCommandType createOrShowCommand);

    protected abstract string MakeSubstitutions(TSubstitutionsType markdownSubstitutions, string fileContents);
}
