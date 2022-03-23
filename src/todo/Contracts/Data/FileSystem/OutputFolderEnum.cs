using System;

namespace Todo.Contracts.Data.FileSystem;

[Flags]
public enum OutputFolderEnum
{
    MainFolder = 1,
    ArchiveFolder = 2
}
