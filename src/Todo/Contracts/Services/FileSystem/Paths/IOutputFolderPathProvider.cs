namespace Todo.Contracts.Services.FileSystem.Paths;

public interface IOutputFolderPathProvider
{
    string GetRootedOutputFolder();

    string GetRootedArchiveFolder();
}
