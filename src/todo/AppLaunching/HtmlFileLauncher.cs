using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.AppLaunching;

public class HtmlFileLauncher : IHtmlFileLauncher
{

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.I4)]
    private static extern int SetForegroundWindow(IntPtr hwnd);

    private readonly IConfigurationProvider _configurationProvider;
    
    private readonly IPathHelper _pathHelper;

    private readonly IOutputWriter _outputWriter;

    public HtmlFileLauncher(IConfigurationProvider configurationProvider, 
        IPathHelper pathHelper, IOutputWriter outputWriter)
    {
        _configurationProvider = configurationProvider;
        _pathHelper = pathHelper;
        _outputWriter = outputWriter;
    }

    public void LaunchFiles(params string [] paths)
    {
        foreach (var path in paths)
        {
            LaunchSingleFile(path);
        }
    }

    private void LaunchSingleFile(string path)
    {
        var browserLaunchInfo = _configurationProvider.Config.BrowserLaunch.GetPathForThisOs();

        var browserPath = _pathHelper.ResolveIfNotRooted(browserLaunchInfo.Path);

        var parameters = browserLaunchInfo.InterpolateParameters(path);

        _outputWriter.WriteLine($"Opening {path} in a browser.");

        var process = Process.Start(browserPath, parameters);

        BringMainWindowToFrontIfWindows(process);
    }

    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    private enum ShowWindowEnum
    {
        Hide = 0,
        ShowNormal = 1, ShowMinimized = 2,
        Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
        Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
        Restore = 9, ShowDefault = 10, ForceMinimized = 11
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    private static int BringMainWindowToFrontIfWindows(Process process)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return 0;

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
