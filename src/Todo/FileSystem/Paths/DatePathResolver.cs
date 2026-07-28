using System;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Contracts.Services.StateAndConfig;

namespace Todo.FileSystem.Paths;

public sealed class DateListPathResolver(
    IConfigurationProvider configurationProvider,
    IOutputFolderPathProvider outputFolderPathProvider,
    IFilenameDateParser filenameDateParser)
    : PathResolverBase<DateOnly>(configurationProvider, outputFolderPathProvider), 
        IDateListPathResolver
{
    protected override FilePathInfo GetFilePathInfo(string fileName, string formattedPath, 
        FileTypeEnum fileType, FolderEnum folderType)
    {
        if (fileType != FileTypeEnum.MarkdownDayList && fileType != FileTypeEnum.Html)
        {
            throw new Exception($"{nameof(DateListPathResolver)} should be used only for " +
                                $"files of type {nameof(FileTypeEnum.MarkdownDayList)} or " +
                                $"{nameof(FileTypeEnum.Html)}");
        }

        if (!filenameDateParser.TryParse(fileName, out var date))
        {
            throw new Exception($"Expecting file of type {nameof(FileTypeEnum.MarkdownDayList)} " +
                                $"but no date parsed from file {fileName}");
        }

        return DayListFilePathInfo.Of(formattedPath, fileType, folderType, date);
    }

    protected override string FileNameWithoutExtension(DateOnly dateOnly)
    {
        var fileNameFragments = GetFragments(
            ConfigurationProvider.ConfigInfo.Configuration.TodoListFilenameFormatWithoutExension,
            '{', '}', dateOnly.ToString);

        return string.Join("", fileNameFragments);
    }
}