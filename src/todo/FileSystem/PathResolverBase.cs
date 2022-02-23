using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem;

[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
public abstract class PathResolverBase<TParameterType> : IPathResolver<TParameterType>
{
    protected readonly IConfigurationProvider ConfigurationProvider;
    private readonly IPathHelper _pathHelper;

    protected const string MarkdownExtension = "md";
    protected const string HtmlExtension = "html";
    protected const string SettingsExtension = "json";

    protected PathResolverBase(IConfigurationProvider configurationProvider, 
        IPathHelper pathHelper)
    {
        ConfigurationProvider = configurationProvider;
        _pathHelper = pathHelper;
    }

    public abstract string GetRegExForThisFileType();

    public abstract string FileNameWithoutExtension(TParameterType parameter);

    public string FileNameFor(TParameterType parameter, FileTypeEnum fileType)
        => $"{FileNameWithoutExtension(parameter)}.{GetExtension(fileType)}";

    public FilePathInfo ResolvePathFor(TParameterType parameter, FileTypeEnum fileType, bool allowNotPresent)
    {
        var pathInTodoFolder = GetFilePathFor(parameter, fileType);
        var pathInArchiveFolder  = GetArchiveFilePathFor(parameter, fileType);

        return CheckExistsAndGetFilePathInfo(pathInTodoFolder, pathInArchiveFolder, allowNotPresent);
    }

    public FilePathInfo GetFilePathFor(TParameterType parameter, FileTypeEnum fileType)
    {
        var fileName = FileNameFor(parameter, fileType);
        var path = Path.Combine(ConfigurationProvider.Config.OutputFolder, fileName);

        return FilePathInfo.Of(path, fileType, FolderEnum.TodoRoot);
    }

    public FilePathInfo GetArchiveFilePathFor(TParameterType parameter, FileTypeEnum fileType)
    {
        var fileName = FileNameFor(parameter, fileType);
        var path = Path.Combine(ConfigurationProvider.Config.OutputFolder,
            ConfigurationProvider.Config.ArchiveFolderName, fileName);

        return FilePathInfo.Of(path, fileType, FolderEnum.Archive);
    }


    protected static string GetExtension(FileTypeEnum fileTypeEnum)
        => fileTypeEnum switch
        {
            FileTypeEnum.Html => HtmlExtension,
            FileTypeEnum.Markdown => MarkdownExtension,

            FileTypeEnum.HtmlTemplate => HtmlExtension,
            FileTypeEnum.MarkdownTemplate => MarkdownExtension,

            FileTypeEnum.Settings => SettingsExtension,

            _ => throw new ArgumentOutOfRangeException(nameof(fileTypeEnum), fileTypeEnum, null)
        };

    protected static IEnumerable<string> GetFragments(string str, char openChar, char closeChar,
        Func<string, string> operatorForEnclosedFragments)
    {
        var enclosed = false;
        var previousIndex = 0;

        for (var i = 0; i < str.Length; i++)
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

    protected FilePathInfo CheckExistsAndGetFilePathInfo(FilePathInfo pathInTodoFolder,
        FilePathInfo pathInArchiveFolder, bool allowNotPresent)
        => (File.Exists(pathInTodoFolder.Path), File.Exists(pathInArchiveFolder.Path), allowNotPresent) switch
        {
            (true, false, _) or (false, false, true) => pathInTodoFolder, //Exists in todo root or in neither

            (false, false, false) => throw new Exception(
                "File not found in either todo root folder or the archive folder"),

            (false, true, _) => pathInArchiveFolder, //Exists in archive but not in todo root

            (true, true, _) => throw new Exception(
                "File found in both the todo root folder and the archive folder") //Exists in both
        };
}
