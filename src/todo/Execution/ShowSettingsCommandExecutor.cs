using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowSettingsCommandExecutor: CommandExecutorBase<ShowSettingsCommand>, IShowSettingsCommandExecutor
{
    private readonly ISettingsPathProvider _settingsPathProvider;
    private readonly IFileOpener _fileOpener;

    public ShowSettingsCommandExecutor(ISettingsPathProvider settingsPathProvider,
        IFileOpener fileOpener, IOutputWriter outputWriter)
        : base(outputWriter)
    {
        _settingsPathProvider = settingsPathProvider;
        _fileOpener = fileOpener;
    }

    public override void Execute(ShowSettingsCommand _)
    {
        var path = _settingsPathProvider.GetSettingsPathInHierarchy().Path;

        OutputWriter.WriteLine($"Opening {path}");

        _fileOpener.LaunchFilesInDefaultEditor(path);
    }
}
