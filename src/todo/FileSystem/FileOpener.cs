using System.Diagnostics;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem;

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
