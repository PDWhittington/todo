using System;
using Todo.Contracts.Data.FileSystem;
using Todo.FileSystem;

namespace Todo.Templates;

public abstract class TemplateProviderBase : FileReader
{
    protected abstract FilePathInfo GetTemplatePath();

    /// <summary>
    /// Returns a string representing the Markdown template
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public string GetTemplate()
    {
        var pathInfo = GetTemplatePath();
        return GetFileText(pathInfo.Path);
    }
}
