namespace Todo.Contracts.Services.FileSystem;

public interface IFileOpener
{
    void LaunchFileInDefaultEditor(string path);
}
