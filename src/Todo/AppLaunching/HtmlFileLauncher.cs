using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.AppLaunching;

public partial class HtmlFileLauncher(
    IConfigurationProvider configurationProvider,
    IPathHelper pathHelper,
    IOutputWriter outputWriter,
    ILaunchInfoSelector launchInfoSelector
) : IHtmlFileLauncher
{
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    // ReSharper disable once UnusedMethodReturnValue.Local
    private static partial bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.I4)]
    private static partial int SetForegroundWindow(IntPtr hwnd);

    public void LaunchFiles(params string[] paths)
    {
        foreach (var path in paths)
        {
            LaunchSingleFile(path);
        }
    }

    private void LaunchSingleFile(string path)
    {
        var browserLaunchInfos = configurationProvider.ConfigInfo.Configuration.BrowserPath;
        var browserLaunchInfo = launchInfoSelector.SelectLaunchInfoForThisOs(browserLaunchInfos);
        var browserPath = pathHelper.ResolveIfNotRooted(browserLaunchInfo.Path);

        var pathWithFileProtocol = new Uri(path);
        var parameters = browserLaunchInfo.InterpolateParameters(pathWithFileProtocol.ToString());

        outputWriter.WriteLine($"Launching {browserPath} {parameters}");

        var process = Process.Start(browserPath, parameters);
        BringMainWindowToFrontIfWindows(process);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private enum ShowWindowEnum
    {
        Hide = 0,
        ShowNormal = 1,
        ShowMinimized = 2,
        Maximize = 3,
        ShowNormalNoActivate = 4,
        Show = 5,
        Minimize = 6,
        ShowMinNoActivate = 7,
        ShowNoActivate = 8,
        Restore = 9,
        ShowDefault = 10,
        ForceMinimized = 11
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    private static int BringMainWindowToFrontIfWindows(Process process)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return 0;

        // check if the window is hidden / minimized
        if (process.MainWindowHandle == IntPtr.Zero)
        {
            // the window is hidden so try to restore it before setting focus.
            ShowWindow(process.Handle, ShowWindowEnum.ShowDefault);
        }

        // set user the focus to the window
        return SetForegroundWindow(process.MainWindowHandle);
    }
}