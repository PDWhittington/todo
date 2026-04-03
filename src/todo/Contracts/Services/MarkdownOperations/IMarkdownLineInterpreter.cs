using Todo.Contracts.Data.Markdown;

namespace Todo.Contracts.Services.MarkdownOperations;

public interface IMarkdownLineInterpreter
{
    MarkdownLineInfo CreateMarkdownLine(string line);
}