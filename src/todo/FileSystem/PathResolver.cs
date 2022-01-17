using System;
using System.Collections.Generic;
using System.IO;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem;

public class PathResolver : IPathResolver
{
    private readonly IConfigurationProvider _configurationProvider;

    private const string MarkdownExtension = "md";
    private const string HtmlExtension = "html";
    private const string SettingsExtension = "json";

    public PathResolver(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider;
    }

    public string FileNameWithoutExtension(DateOnly dateOnly)
    {
        var fileNameFragments = GetFragments(_configurationProvider.Config.TodoListFilenameFormat,
            '{', '}', str => dateOnly.ToString(str));

        return string.Join("", fileNameFragments);
    }

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

    public string GetOutputFolder() => _configurationProvider.Config.OutputFolder;

    public string GetArchiveFolder() => Path.Combine(_configurationProvider.Config.OutputFolder,
        _configurationProvider.Config.ArchiveFolderName);

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

    private static IEnumerable<string> GetFragments(string str, char openChar, char closeChar,
        Func<string, string> operatorForEnclosedFragments)
    {
        var enclosed = false;
        var previousIndex = 0;

        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == openChar)
            {
                if (enclosed) throw new Exception("OpenChar and CloseChar must alternate");

                yield return str.Substring(previousIndex, i - previousIndex);

                previousIndex = i + 1;
                enclosed = true;
            }
            else if (str[i] == closeChar)
            {
                if (!enclosed) throw new Exception("OpenChar and CloseChar must alternate");

                yield return operatorForEnclosedFragments(str.Substring(previousIndex, i - previousIndex));

                previousIndex = i + 1;

                enclosed = false;
            }
        }

        yield return enclosed
            ? operatorForEnclosedFragments(str[previousIndex..])
            : str[previousIndex..];
    }
}
