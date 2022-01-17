using System.Diagnostics;
using Todo.Contracts.Services.FileLaunching;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileLaunching;

public class FileOpener : IFileOpener
{
    private readonly IConfigurationProvider _configurationProvider;

    public FileOpener(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }

    public void LaunchFileInDefaultEditor(string path)
    {
        Process.Start(_configurationProvider.Config.TextEditorPath, path);
    }
}
