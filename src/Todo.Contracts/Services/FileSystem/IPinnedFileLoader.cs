using Todo.Contracts.Data.Memory;

namespace Todo.Contracts.Services.FileSystem;

public interface IPinnedFileLoader
{
    UnmanagedByteArray LoadFile(string filePath);
}