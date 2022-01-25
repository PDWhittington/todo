using System;
using Todo.Contracts.Data.FileSystem;
using Todo.FileSystem;

namespace Todo.Templates;

public abstract class TemplateProviderBase<TKeyType> : FileReaderBase
    where TKeyType : Enum
{
    protected abstract FilePathInfo GetTemplatePath(TKeyType key);

    /// <summary>
    /// Returns a string representing the Markdown template
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public TodoFile GetTemplate(TKeyType key)
    {
        var filePathInfo = GetTemplatePath(key);

        return TodoFile.Of(filePathInfo, GetFileText(filePathInfo.Path));
    }
}
