using System;
using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.FileSystem;

public interface IContentFileResolver
{
    FilePathInfo GetPathFor(DateOnly date, FileTypeEnum fileType);
}
