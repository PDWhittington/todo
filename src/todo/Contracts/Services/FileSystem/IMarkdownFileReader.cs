using System;
using Todo.Contracts.Data.FileSystem;

namespace Todo.Contracts.Services.FileSystem;

public interface IMarkdownFileReader
{
    TodoFile ReadMarkdownFile(DateOnly dateOnly);
}
