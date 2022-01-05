using System;
using System.IO;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem;

public class FileNamer : IFileNamer
{
    private readonly IConfigurationProvider _configurationProvider;

    private const string MarkdownExtension = "md";
    private const string HtmlExtension = "html";
    private const string SettingsExtension = "json";

    public FileNamer(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }

    public string FileNameWithoutExtension(DateOnly dateOnly) => $"todo-{dateOnly:yyyy-MM-dd}";

    public string FileNameForDate(DateOnly dateOnly, FileTypeEnum fileType) => $"{FileNameWithoutExtension(dateOnly)}.{GetExtension(fileType)}";

    public FilePathInfo GetFilePath(DateOnly dateOnly, FileTypeEnum fileType)
    {
        var fileName = FileNameForDate(dateOnly, fileType);
        var path = Path.Combine(_configurationProvider.Config.OutputFolder, fileName);

        return FilePathInfo.Of(path, fileType, FolderEnum.TodoRoot);
    }

    public FilePathInfo GetArchiveFilePath(DateOnly dateOnly, FileTypeEnum fileType)
    {
        var fileName = FileNameForDate(dateOnly, fileType);
        var path = Path.Combine(_configurationProvider.Config.OutputFolder,
            _configurationProvider.Config.ArchiveFolderName, fileName);

        return FilePathInfo.Of(path, fileType, FolderEnum.Archive);
    }

    private static string GetExtension(FileTypeEnum fileTypeEnum)
        => fileTypeEnum switch
        {
            FileTypeEnum.Html => HtmlExtension,
            FileTypeEnum.Markdown => MarkdownExtension,

            FileTypeEnum.HtmlTemplate => HtmlExtension,
            FileTypeEnum.MarkdownTemplate => MarkdownExtension,

            FileTypeEnum.Settings => SettingsExtension,

            _ => throw new ArgumentOutOfRangeException(nameof(fileTypeEnum), fileTypeEnum, null)
        };
}
