using Todo.Contracts.Data.Markdown;
using Todo.Contracts.Data.Memory;

namespace Todo.Contracts.Services.MarkdownOperations;

public interface IMarkdownLineInterpreter
{
    MarkdownLineInfo[] CreateMarkdownLines(UnmanagedByteArray file);
}