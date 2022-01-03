﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.Execution;
using Todo.Contracts.Services.FileNaming;
using Todo.Contracts.Services.StateAndConfig;
using Todo.FileNaming;

namespace Todo.Execution;

public class ShowHtmlExecutor : IShowHtmlExecutor
{
    [DllImport("user32.dll")]
    public static extern int SetForegroundWindow(int hwnd); 
    
    [DllImport("user32.dll")]
    [return: MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
    private static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

    [DllImport("user32.dll")]
    private static extern int SetForegroundWindow(IntPtr hwnd);
    
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IFileNamer _fileNamer;

    public ShowHtmlExecutor(IConfigurationProvider configurationProvider, IFileNamer fileNamer)
    {
        _configurationProvider = configurationProvider;
        _fileNamer = fileNamer;
    }

    public void Execute(ShowHtmlCommand showHtmlCommand)
    {
        var configuration = _configurationProvider.GetConfiguration();
        var path = _fileNamer.GetFilePath(showHtmlCommand.Date, FileTypeEnum.Html);

        if (!File.Exists(path))
            throw new Exception(
                $"Path {path} does not exist. You may need to export to Html in VSCode first");

        var process = Process.Start(configuration.BrowserPath, path);
        
        BringMainWindowToFront(process);
    }
    
    
    private enum ShowWindowEnum
    {
        Hide = 0,
        ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
        Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
        Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
        Restore = 9, ShowDefault = 10, ForceMinimized = 11
    };

    public void BringMainWindowToFront(Process process)
    {
        // check if the window is hidden / minimized
        if (process.MainWindowHandle == IntPtr.Zero)
        {
            // the window is hidden so try to restore it before setting focus.
            ShowWindow(process.Handle, ShowWindowEnum.ShowDefault);
        }

        // set user the focus to the window
        SetForegroundWindow(process.MainWindowHandle);
    }
}