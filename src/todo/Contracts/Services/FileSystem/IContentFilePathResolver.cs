using System;
using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.FileSystem;

public interface IContentFilePathResolver
{
    FilePathInfo GetPathFor(DateOnly date, FileTypeEnum fileType, bool allowNotPresent);
}
