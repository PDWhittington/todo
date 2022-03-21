using System.Diagnostics;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.AppLaunching;

public class TextFileLauncher : ITextFileLauncher
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IPathHelper _pathHelper;
    private string? _textEditorPath;

    public TextFileLauncher(IConfigurationProvider configurationProvider, IPathHelper pathHelper)
    {
        _configurationProvider = configurationProvider;
        _pathHelper = pathHelper;
    }

    private string TextEditorPath => _textEditorPath ?? GetTextEditorPath();

    private string GetTextEditorPath()
    {
        _textEditorPath = _pathHelper.ResolveIfNotRooted(
            _configurationProvider.ConfigInfo.Configuration.TextEditorPath.GetPathForThisOs().Path);
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
