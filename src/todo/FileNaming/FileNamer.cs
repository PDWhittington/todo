using System;
using System.IO;
using Todo.Contracts.Services.FileNaming;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileNaming;

public class FileNamer : IFileNamer
{
    private readonly IConfigurationProvider _configurationProvider;

    private readonly string _markdownExtension = "md";
    private readonly string _htmlExtension = "html";

    public FileNamer(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }

    public string FileNameWithoutExtension(DateOnly dateOnly) => $"todo-{dateOnly:yyyy-MM-dd}";
    
    public string FileNameForDate(DateOnly dateOnly, FileTypeEnum fileType) => $"{FileNameWithoutExtension(dateOnly)}.{GetExtension(fileType)}";

    public string GetFilePath(DateOnly dateOnly, FileTypeEnum fileType)
    {
        var configuration = _configurationProvider.GetConfiguration();
        var fileName = FileNameForDate(dateOnly, fileType);
        return Path.Combine(configuration.OutputFolder, fileName);
    }
    
    public string GetArchiveFilePath(DateOnly dateOnly, FileTypeEnum fileType)
    {
        var configuration = _configurationProvider.GetConfiguration();
        var fileName = FileNameForDate(dateOnly, fileType);
        return Path.Combine(configuration.OutputFolder, configuration.ArchiveFolderName, fileName);
    }

    private string GetExtension(FileTypeEnum fileTypeEnum)
        => fileTypeEnum switch
        {
            FileTypeEnum.Html => _htmlExtension,
            FileTypeEnum.Markdown => _markdownExtension,
            _ => throw new Exception("FileType not recognised"),
        };
}