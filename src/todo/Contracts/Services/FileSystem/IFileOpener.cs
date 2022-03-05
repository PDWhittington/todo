namespace Todo.Contracts.Services.FileSystem;

public interface IFileOpener
{
    void LaunchFilesInDefaultEditor(params string [] paths);
}
