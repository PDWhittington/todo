using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.Reporting;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class InitCommandExecutor : CommandExecutorBase<InitCommand>, IInitCommandExecutor
{
    private readonly IConstantsProvider _constantsProvider;
    private readonly ISettingsPathProvider _settingsPathProvider;

    public InitCommandExecutor(IConstantsProvider constantsProvider,
        ISettingsPathProvider settingsPathProvider, IOutputWriter outputWriter)
        : base(outputWriter)
    {
        _constantsProvider = constantsProvider;
        _settingsPathProvider = settingsPathProvider;
    }

    public override void Execute(InitCommand _)
    {
        var settingsPath = _settingsPathProvider.GetSettingsPathInWorkingFolder().Path;

        OutputWriter.WriteLine($"Initialising folder for todo. Creating {settingsPath}");

        var settingsFileStream = Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(_constantsProvider.DefaultSettingsFile.FullName);

        if (settingsFileStream == null) throw new Exception(
                "Cannot retrieve default settings file from executable");

        using var outputStream = new FileStream(settingsPath, FileMode.Create, FileAccess.Write);

        settingsFileStream.CopyTo(outputStream);
    }
}
