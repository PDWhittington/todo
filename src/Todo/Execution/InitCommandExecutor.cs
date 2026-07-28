using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class InitCommandExecutor(
    IConstantsProvider constantsProvider,
    IManifestStreamProvider manifestStreamProvider,
    ISettingsPathProvider settingsPathProvider,
    IOutputWriter outputWriter,
    IConfigurationProvider configurationProvider,
    IFolderCreator folderCreator, ILogger<InitCommandExecutor> logger)
    : CommandExecutorBase<InitCommand>(outputWriter, logger), IInitCommandExecutor
{
    public override void Execute(InitCommand _)
    {
        var settingsPath = settingsPathProvider.GetSettingsPathInWorkingFolder().Path;

        OutputWriter.WriteLine($"Initialising folder for todo. Creating {settingsPath}");

        manifestStreamProvider.WriteStringFromManifestToFile(
            constantsProvider.DefaultSettingsFile.FullName,
            settingsPath);

        configurationProvider.Reset();

        folderCreator.CreateOutputFolder();
        folderCreator.CreateArchiveFolder();
    }
}