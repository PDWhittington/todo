using System;

namespace Todo.Contracts.Services.FileSystem;

public interface IMarkdownFileReader
{
    string ReadMarkdownFile(DateOnly dateOnly);
}
