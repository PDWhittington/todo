using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.AssemblyOperations;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class InitCommandExecutor : CommandExecutorBase<InitCommand>, IInitCommandExecutor
{
    private readonly IConstantsProvider _constantsProvider;
    private readonly IManifestStreamProvider _manifestStreamProvider;
    private readonly ISettingsPathProvider _settingsPathProvider;
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IOutputFolderPathProvider _outputFolderPathProvider;

    public InitCommandExecutor(IConstantsProvider constantsProvider, IManifestStreamProvider manifestStreamProvider,
        ISettingsPathProvider settingsPathProvider, IOutputWriter outputWriter,
        IConfigurationProvider configurationProvider, IOutputFolderPathProvider outputFolderPathProvider)
        : base(outputWriter)
    {
        _constantsProvider = constantsProvider;
        _manifestStreamProvider = manifestStreamProvider;
        _settingsPathProvider = settingsPathProvider;
        _configurationProvider = configurationProvider;
        _outputFolderPathProvider = outputFolderPathProvider;
    }

    public override void Execute(InitCommand _)
    {
        var settingsPath = _settingsPathProvider.GetSettingsPathInWorkingFolder().Path;

        OutputWriter.WriteLine($"Initialising folder for todo. Creating {settingsPath}");

        _manifestStreamProvider.WriteStringFromManifestToFile(_constantsProvider.DefaultSettingsFile.FullName,
            settingsPath);

        _configurationProvider.Reset();

        var outputFolder = _outputFolderPathProvider.GetRootedOutputFolder();

        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        var archiveFolder = _outputFolderPathProvider.GetRootedArchiveFolder();

        if (!Directory.Exists(archiveFolder))
        {
            Directory.CreateDirectory(archiveFolder);
        }
    }
}
