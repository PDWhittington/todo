using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Todo.Contracts.Data.Config;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.AppLaunching;

public partial class HtmlFileLauncher(
    IConfigurationProvider configurationProvider,
    IPathHelper pathHelper,
    IOutputWriter outputWriter,
    ILaunchInfoSelector launchInfoSelector,
    IProcessLauncher processLauncher)
    : AppLauncherBase(pathHelper, outputWriter, launchInfoSelector, processLauncher), IHtmlFileLauncher
{
    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    // ReSharper disable once UnusedMethodReturnValue.Local
    private static partial bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.I4)]
    private static partial int SetForegroundWindow(IntPtr hwnd);

    protected override PerOsLaunchInfos GetLaunchInfos()
        => configurationProvider.ConfigInfo.Configuration.BrowserPath;

    protected override string GetPathToLaunch(string path)
        => new Uri(path).ToString();

    protected override void AfterLaunch(Process? process)
    {
        if (process is not null)
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
