using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;

namespace Todo.Contracts.Services.MarkdownOperations;

public interface IMarkdownLineInterpreter
{
    // MarkdownLineInfo [] CreateMarkdownLine(FilePathInfo filePathInfo, string [] lines);

    MarkdownLineInfo[] CreateMarkdownLine(FilePathInfo filePathInfo, byte[] bytes);
}