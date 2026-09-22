using System.Diagnostics;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.UI;

namespace Todo.AppLaunching;

public abstract class AppLauncherBase(
    IPathHelper pathHelper,
    IOutputWriter outputWriter,
    ILaunchInfoSelector launchInfoSelector,
    IProcessLauncher processLauncher) : IAppLauncher
{
    protected IOutputWriter OutputWriter { get; } = outputWriter;

    public void LaunchFiles(params string[] paths)
    {
        var launchInfos = GetLaunchInfos();
        var launchInfoForThisOs = launchInfoSelector.SelectLaunchInfoForThisOs(launchInfos);
        var executablePath = pathHelper.ResolveIfNotRooted(launchInfoForThisOs.Path);

        foreach (var path in paths)
        {
            var pathToLaunch = GetPathToLaunch(path);
            var parameters = launchInfoForThisOs.InterpolateParameters(pathToLaunch);

            WriteLaunchMessage(path, executablePath, parameters);

            var process = processLauncher.Start(executablePath, parameters);
            AfterLaunch(process);
        }
    }

    protected abstract PerOsLaunchInfos GetLaunchInfos();

    protected virtual string GetPathToLaunch(string path) => path;

    protected virtual void WriteLaunchMessage(string originalPath, string executablePath, string parameters)
        => OutputWriter.WriteLine($"Launching {executablePath} {parameters}");

    protected virtual void AfterLaunch(Process? process)
    {
    }
}
