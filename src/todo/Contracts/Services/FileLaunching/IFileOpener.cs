namespace Todo.Contracts.Services.FileLaunching;

public interface IFileOpener
{
    void LaunchFileInDefaultEditor(string path);
}
