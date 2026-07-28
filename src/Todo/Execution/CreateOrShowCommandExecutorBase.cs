using System.IO;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Git.Commands;
using Todo.Contracts.Data.Git.Results;
using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Memory;
using Todo.Contracts.Data.Substitutions;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.Git;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

public abstract class CreateOrShowCommandExecutorBase<TCommandType, TSubstitutionsType>(
    IConfigurationProvider configurationProvider,
    IGitInterface gitInterface,
    ITextFileLauncher fileOpener,
    IOutputWriter outputWriter,
    IFolderCreator folderCreator,
    ILogger<CreateOrShowCommandExecutorBase<TCommandType, TSubstitutionsType>> logger)
    : CommandExecutorBase<TCommandType>(outputWriter, logger)
    where TCommandType : CreateOrShowCommandBase
    where TSubstitutionsType : MarkdownSubstitutionsBase
{
    public override void Execute(TCommandType createOrShowCommand)
    {
        Logger.LogInformation("Entered {GetType}.{MethodName}", GetType(), nameof(Execute));
        
        var pathInfo = GetFilePathInfo(createOrShowCommand);

        if (!File.Exists(pathInfo.Path))
        {
            var templateFile = GetTemplate();

            var markdownSubstitutions = GetMarkdownSubstitutions(createOrShowCommand);

            folderCreator.CreateFromPathIfDoesntExist(pathInfo.Path);
            using var stream = File.Create(pathInfo.Path);
            
            MakeSubstitutions(markdownSubstitutions, templateFile.FileContents, stream);

            if (configurationProvider.ConfigInfo.Configuration.UseGit)
            {
                gitInterface.RunGitCommand<GitAddCommand, VoidResult>(new GitAddCommand(pathInfo.Path));
            }
        }

        fileOpener.LaunchFiles(pathInfo.Path);
    }

    protected abstract FilePathInfo GetFilePathInfo(TCommandType createOrShowCommand);

    protected abstract TodoFile GetTemplate();

    protected abstract TSubstitutionsType GetMarkdownSubstitutions(TCommandType createOrShowCommand);

    protected abstract void MakeSubstitutions(TSubstitutionsType markdownSubstitutions, 
        UnmanagedByteArray fileContents, Stream stream);
}
