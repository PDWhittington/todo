using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.FileSystem;

namespace Todo.Execution;

public abstract class CreateOrShowCommandExecutorBase<TCommandType, TSubstitutionsType> : CommandExecutorBase<TCommandType>
    where TCommandType : CreateOrShowCommandBase
    where TSubstitutionsType : MarkdownSubstitutionsBase
{
    private readonly IFileOpener _fileOpener;

    protected CreateOrShowCommandExecutorBase(IFileOpener fileOpener)
    {
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
        }

        _fileOpener.LaunchFileInDefaultEditor(pathInfo.Path);
    }

    protected abstract FilePathInfo GetFilePathInfo(TCommandType createOrShowCommand);

    protected abstract TodoFile GetTemplate();

    protected abstract TSubstitutionsType GetMarkdownSubstitutions(TCommandType createOrShowCommand);

    protected abstract string MakeSubstitutions(TSubstitutionsType markdownSubstitutions, string fileContents);
}
