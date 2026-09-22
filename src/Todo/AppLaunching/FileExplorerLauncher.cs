using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.AppLaunching;

public class FileExplorerLauncher(
    IPathHelper pathHelper,
    IOutputWriter outputWriter,
    ILaunchInfoSelector launchInfoSelector,
    IProcessLauncher processLauncher)
    : AppLauncherBase(pathHelper, outputWriter, launchInfoSelector, processLauncher), IFileExplorerLauncher
{
    private static readonly PerOsLaunchInfos FileExplorerPaths = new(
        new ProcessLaunchInfo("explorer.exe", "{0}"),
        new ProcessLaunchInfo("xdg-open", "{0}"),
        new ProcessLaunchInfo("open", "{0}"));

    protected override PerOsLaunchInfos GetLaunchInfos() => FileExplorerPaths;

    protected override void WriteLaunchMessage(string originalPath, string executablePath, string parameters)
    {
        OutputWriter.WriteLine($"Opening {originalPath} in the system file manager.");
        OutputWriter.WriteLine($"({executablePath} {parameters})");
    }
}
