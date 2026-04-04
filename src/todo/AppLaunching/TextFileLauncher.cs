using System.Diagnostics;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.AppLaunching;

public class TextFileLauncher(IConfigurationProvider configurationProvider, IPathHelper pathHelper)
    : ITextFileLauncher
{
    private string? _textEditorPath;

    private string TextEditorPath => _textEditorPath ?? GetTextEditorPath();

    private string GetTextEditorPath()
    {
        _textEditorPath = pathHelper.ResolveIfNotRooted(
            configurationProvider.ConfigInfo.Configuration.TextEditorPath.GetPathForThisOs().Path);
        return _textEditorPath;
    }

    public void LaunchFiles(params string [] paths)
    {
        foreach (var path in paths)
        {
            Process.Start(TextEditorPath, path);
        }
    }
}
