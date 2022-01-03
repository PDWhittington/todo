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

    public string FileNameForDate(DateOnly dateOnly) => $"todo-{dateOnly:yyyy-MM-dd}.md";

    public string FilePathForDate(DateOnly dateOnly)
    {
        var configuration = _configurationProvider.GetConfiguration();

        var fileName = FileNameForDate(dateOnly);
        
        return Path.Combine(configuration.OutputFolder, fileName);
    }
    
    public string ArchiveFilePathForDate(DateOnly dateOnly)
    {
        var configuration = _configurationProvider.GetConfiguration();

        var fileName = FileNameForDate(dateOnly);
        
        return Path.Combine(configuration.OutputFolder, configuration.ArchiveFolderName, fileName);
    }
}