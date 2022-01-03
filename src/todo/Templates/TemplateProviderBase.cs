using System;
using Todo.FileSystem;

namespace Todo.Templates;

public abstract class TemplateProviderBase : FileReader
{
    protected abstract string GetTemplatePath();

    /// <summary>
    /// Returns a string representing the Markdown template
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public string GetTemplate()
    {
        var rootedPath = GetTemplatePath();
        return GetFileText(rootedPath);
    }
}
