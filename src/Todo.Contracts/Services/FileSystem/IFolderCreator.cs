namespace Todo.Contracts.Services.FileSystem;

public interface IFolderCreator
{
    void CreateOutputFolder();

    void CreateArchiveFolder();

    // ReSharper disable once UnusedMemberInSuper.Global
    void CreateIfDoesntExist(string directory);

    void CreateFromPathIfDoesntExist(string path);
}