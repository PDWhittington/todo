using System;
using System.IO;

namespace Todo.Templates;

public abstract class TemplateProviderBase
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

        if (!File.Exists(rootedPath)) throw new Exception($"{rootedPath} not found");

        return File.ReadAllText(rootedPath);
    }
}
