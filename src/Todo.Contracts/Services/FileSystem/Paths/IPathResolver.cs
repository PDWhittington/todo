using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.FileSystem.Paths;

public interface IPathResolver<in TParameterType>
{
    // ReSharper disable once UnusedMemberInSuper.Global
    string FileNameFor(TParameterType parameter, FileTypeEnum fileType);
    
    bool TryResolvePathFor(TParameterType parameter, FileTypeEnum fileType, out FilePathInfo? filePathInfo);

    FilePathInfo ResolvePathFor(TParameterType parameter, FileTypeEnum fileType, bool allowNotPresent);

    FilePathInfo GetFilePathFor(TParameterType parameter, FileTypeEnum fileType);

    public FilePathInfo GetArchiveFilePathFor(TParameterType parameter, FileTypeEnum fileType);
}