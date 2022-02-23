using System.Diagnostics;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem;

public class FileOpener : IFileOpener
{
    private readonly IConfigurationProvider _configurationProvider;
    private readonly IEnvironmentPathResolver _environmentPathResolver;
    private string? _textEditorPath;

    public FileOpener(IConfigurationProvider configurationProvider,
        IEnvironmentPathResolver environmentPathResolver)
    {
        _configurationProvider = configurationProvider;
        _environmentPathResolver = environmentPathResolver;
    }

    private string TextEditorPath => _textEditorPath ?? GetTextEditorPath();

    private string GetTextEditorPath()
    {
        _textEditorPath = _environmentPathResolver.ResolveIfNotRooted(
            _configurationProvider.Config.TextEditorPath.GetPathForThisOs());
        return _textEditorPath;
    }

    public void LaunchFileInDefaultEditor(string path)
    {
        Process.Start(TextEditorPath, path);
    }
}
