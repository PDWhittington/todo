using System.Diagnostics.CodeAnalysis;
using System.IO;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.Execution;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowSettingsCommandExecutor: CommandExecutorBase<ShowSettingsCommand>, IShowSettingsCommandExecutor
{
    private readonly ISettingsPathProvider _settingsPathProvider;
    private readonly IFileOpener _fileOpener;
    private readonly IConstantsProvider _constantsProvider;

    public ShowSettingsCommandExecutor(ISettingsPathProvider settingsPathProvider, IFileOpener fileOpener,
        IConstantsProvider constantsProvider)
    {
        _settingsPathProvider = settingsPathProvider;
        _fileOpener = fileOpener;
        _constantsProvider = constantsProvider;
    }

    public override void Execute(ShowSettingsCommand _)
    {
        var path = _settingsPathProvider.GetSettingsPathInHierarchy().Path;

        _fileOpener.LaunchFileInDefaultEditor(path);
    }
}
