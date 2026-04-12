using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;

namespace Todo.FileSystem;

public class FileListCreator(IOutputFolderPathProvider pathRootingProvider, IFilenameDateParser filenameDateParser)
    : IFileListCreator
{
    private record struct PathAndFolder
    {
        public string Path { get; init; }
        public FolderEnum Folder { get; init; }
    }

    public IEnumerable<T> GetFiles<T>(OutputFolderEnum outputFolder, ListFileTypeEnum listFileType)
        where T : FilePathInfo
    {
        var requestedFileInfos = new []
        {
            outputFolder.HasFlag(OutputFolderEnum.MainFolder)
                ? Directory.GetFiles(pathRootingProvider.GetRootedOutputFolder(), "*.md")
                    .Select(path => new PathAndFolder { Path = path, Folder = FolderEnum.TodoRoot })
                : [],

            outputFolder.HasFlag(OutputFolderEnum.ArchiveFolder)
                ? Directory.GetFiles(pathRootingProvider.GetRootedArchiveFolder(), "*.md")
                    .Select(path => new PathAndFolder { Path = path, Folder = FolderEnum.Archive })
                : []

        }.SelectMany(x => x)
        .Select(x => CategoriseAndMatch(x, listFileType))
        .Where(filterInfo => filterInfo.Match)
        .Select(GetFilePathInfo);

        foreach (var pathAndFolder in requestedFileInfos)
        {
            if (pathAndFolder is not T info)
            {
                throw new Exception($"Have encountered a file which is not of type {typeof(T)}");
            }

            yield return info;
        }
    }

    private static FileTypeEnum MapToFileTypeEnum(ListFileTypeEnum lfi)
        => lfi switch
        {
            ListFileTypeEnum.DayList => FileTypeEnum.MarkdownDayList,
            ListFileTypeEnum.TopicList => FileTypeEnum.MarkdownTopicList,
            _ => throw new Exception()
        };

    CategoryAndMatchInfo CategoriseAndMatch(PathAndFolder pathAndFolder, ListFileTypeEnum listFileType)
    {
        var fileName = Path.GetFileName(pathAndFolder.Path);

        var isDayList = filenameDateParser.TryParse(fileName, out var date);

        var match = isDayList && listFileType.HasFlag(ListFileTypeEnum.DayList) ||
                    !isDayList && listFileType.HasFlag(ListFileTypeEnum.TopicList);

        var fileType = isDayList ? ListFileTypeEnum.DayList : ListFileTypeEnum.TopicList;

        return new CategoryAndMatchInfo(match, fileType, date, pathAndFolder);
    }

    private static FilePathInfo GetFilePathInfo(CategoryAndMatchInfo categoryAndMatchInfo)
    {
        if (categoryAndMatchInfo.FileType != ListFileTypeEnum.DayList)
        {
            return FilePathInfo.Of(
                categoryAndMatchInfo.PathAndFolder.Path, MapToFileTypeEnum(categoryAndMatchInfo.FileType),
                categoryAndMatchInfo.PathAndFolder.Folder);
        }
            
        if (categoryAndMatchInfo.Date is null)
        {
            throw new Exception($"Expecting file of type  {nameof(ListFileTypeEnum.DayList)} " +
                                $"but no date is parsed");
        }
                
        return DayListFilePathInfo.Of(
            categoryAndMatchInfo.PathAndFolder.Path, MapToFileTypeEnum(categoryAndMatchInfo.FileType),
            categoryAndMatchInfo.PathAndFolder.Folder, categoryAndMatchInfo.Date.Value);
    }
    
    private record CategoryAndMatchInfo(
        bool Match,
        ListFileTypeEnum FileType,
        DateOnly? Date,
        PathAndFolder PathAndFolder);

}
