using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Data.Markdown;

namespace Todo.Contracts.Services.MarkdownOperations;

public interface IMarkdownFileReader
{
    TodoFile ReadMarkdownFile(DateOnly dateOnly);

    TodoFile ReadMarkdownFile(FilePathInfo filePathInfo);
}
