using System.Diagnostics;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.AppLaunching;

public class FileExplorerLauncher(IPathHelper pathHelper, IOutputWriter outputWriter, 
    ILaunchInfoSelector launchInfoSelector)
    : IFileExplorerLauncher
{
    private static readonly PerOsLaunchInfos FileExplorerPaths = new(
        new ProcessLaunchInfo("explorer.exe", "{0}"),
        new ProcessLaunchInfo("xdg-open", "{0}"),
        new ProcessLaunchInfo("open", "{0}"));

    public void LaunchFiles(params string[] paths)
    {
        foreach (var path in paths)
        {
            LaunchSingleFolder(path);
        }
    }

    private void LaunchSingleFolder(string path)
    {
        var launchInfo = launchInfoSelector.SelectLaunchInfoForThisOS(FileExplorerPaths); 
        var executablePath = pathHelper.ResolveIfNotRooted(launchInfo.Path);
        var parameters = launchInfo.InterpolateParameters(path);

        outputWriter.WriteLine($"Opening {path} in the system file manager.");
        outputWriter.WriteLine($"({executablePath} {parameters})");

        Process.Start(executablePath, parameters);
    }
}