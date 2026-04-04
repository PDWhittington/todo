using System.Diagnostics.CodeAnalysis;
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
    IFolderCreator folderCreator)
    : CommandExecutorBase<InitCommand>(outputWriter), IInitCommandExecutor
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
