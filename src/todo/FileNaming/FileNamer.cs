using System;
using System.IO;
using Todo.Contracts.Services.FileNaming;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileNaming;

public class FileNamer : IFileNamer
{
    private readonly IConfigurationProvider _configurationProvider;

    public FileNamer(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }

    public string FileNameWithoutExtension(DateOnly dateOnly) => $"todo-{dateOnly:yyyy-MM-dd}";
    
    public string MarkdownFileNameForDate(DateOnly dateOnly) => $"{FileNameWithoutExtension(dateOnly)}.md";

    public string MarkdownFilePathForDate(DateOnly dateOnly)
    {
        var configuration = _configurationProvider.GetConfiguration();

        var fileName = MarkdownFileNameForDate(dateOnly);
        
        return Path.Combine(configuration.OutputFolder, fileName);
    }
    
    public string MarkdownArchiveFilePathForDate(DateOnly dateOnly)
    {
        var configuration = _configurationProvider.GetConfiguration();

        var fileName = MarkdownFileNameForDate(dateOnly);
        
        return Path.Combine(configuration.OutputFolder, configuration.ArchiveFolderName, fileName);
    }
}