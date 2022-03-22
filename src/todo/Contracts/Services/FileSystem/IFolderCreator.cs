namespace Todo.Contracts.Services.FileSystem;

public interface IFolderCreator
{
    void CreateOutputFolder();

    void CreateArchiveFolder();

    void CreateIfDoesntExist(string directory);

    void CreateFromPathIfDoesntExist(string path);
}
