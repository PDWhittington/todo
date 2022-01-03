using System;
using System.IO;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem;

public class FileNamer : IFileNamer
{
    private readonly IConfigurationProvider _configurationProvider;

    private const string MarkdownExtension = "md";
    private const string HtmlExtension = "html";

    public FileNamer(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }

    public string FileNameWithoutExtension(DateOnly dateOnly) => $"todo-{dateOnly:yyyy-MM-dd}";

    public string FileNameForDate(DateOnly dateOnly, FileTypeEnum fileType) => $"{FileNameWithoutExtension(dateOnly)}.{GetExtension(fileType)}";

    public string GetFilePath(DateOnly dateOnly, FileTypeEnum fileType)
    {
        var fileName = FileNameForDate(dateOnly, fileType);
        return Path.Combine(_configurationProvider.Config.OutputFolder, fileName);
    }

    public string GetArchiveFilePath(DateOnly dateOnly, FileTypeEnum fileType)
    {
        var fileName = FileNameForDate(dateOnly, fileType);
        return Path.Combine(_configurationProvider.Config.OutputFolder,
            _configurationProvider.Config.ArchiveFolderName, fileName);
    }

    private static string GetExtension(FileTypeEnum fileTypeEnum)
        => fileTypeEnum switch
        {
            FileTypeEnum.Html => HtmlExtension,
            FileTypeEnum.Markdown => MarkdownExtension,
            _ => throw new Exception("FileType not recognised")
        };
}
