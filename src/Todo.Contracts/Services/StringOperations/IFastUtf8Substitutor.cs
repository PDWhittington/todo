using Todo.Contracts.Data.Memory;

namespace Todo.Contracts.Services.StringOperations;

public interface IFastUtf8Substitutor
{
    void CopyToStream(UnmanagedByteArray template, Dictionary<string, string> substitutions, Stream outputStream);
}