using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.Dates;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;
using Todo.Dates;

namespace Todo.FileSystem;

public class FileListCreator : IFileListCreator
{
    private readonly IDateListPathResolver _dateListPathResolver;
    private readonly IOutputFolderPathProvider _pathRootingProvider;
    private readonly IFilenameDateParser _filenameDateParser;

    public FileListCreator(IDateListPathResolver dateListPathResolver,
        IOutputFolderPathProvider pathRootingProvider, IFilenameDateParser filenameDateParser)
    {
        _dateListPathResolver = dateListPathResolver;
        _pathRootingProvider = pathRootingProvider;
        _filenameDateParser = filenameDateParser;
    }

    private record struct PathAndFolder
    {
        public string Path { get; init; }
        public FolderEnum Folder { get; init; }
    }

    public FilePathInfo[] GetFiles(OutputFolderEnum outputFolder,
        ListFileTypeEnum listFileType)
    {
        var pattern = _dateListPathResolver.GetRegExForThisFileType();

        var pathsInRelevantFolders = new []
        {
            outputFolder.HasFlag(OutputFolderEnum.MainFolder)
                ? Directory.GetFiles(_pathRootingProvider.GetRootedOutputFolder(), "*.md")
                    .Select(path => new PathAndFolder { Path = path, Folder = FolderEnum.TodoRoot })
                : [],

            outputFolder.HasFlag(OutputFolderEnum.ArchiveFolder)
                ? Directory.GetFiles(_pathRootingProvider.GetRootedArchiveFolder(), "*.md")
                    .Select(path => new PathAndFolder { Path = path, Folder = FolderEnum.Archive })
                : []

        }.SelectMany(x => x)
        .Select(CategoriseAndMatch)
        .Where(filterInfo => filterInfo.Match)
        .Select(GetFilePathInfo)
        .ToArray();

        return pathsInRelevantFolders;

        FileTypeEnum MapToFileTypeEnum(ListFileTypeEnum lfi)
            => lfi switch
            {
                ListFileTypeEnum.DayList => FileTypeEnum.MarkdownDayList,
                ListFileTypeEnum.TopicList => FileTypeEnum.MarkdownTopicList,
                _ => throw new Exception()
            };

        CategoryAndMatchInfo CategoriseAndMatch(
            PathAndFolder pathAndFolder)
        {
            var fileName = Path.GetFileName(pathAndFolder.Path);

            var isDayList = _filenameDateParser.TryParse(fileName, out var date);

            var match = isDayList && listFileType.HasFlag(ListFileTypeEnum.DayList) ||
                        !isDayList && listFileType.HasFlag(ListFileTypeEnum.TopicList);

            var fileType = isDayList ? ListFileTypeEnum.DayList : ListFileTypeEnum.TopicList;

            return new CategoryAndMatchInfo(match, fileType, date, pathAndFolder);
        }

        FilePathInfo GetFilePathInfo(CategoryAndMatchInfo categoryAndMatchInfo)
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
    }

    private record CategoryAndMatchInfo(
        bool Match,
        ListFileTypeEnum FileType,
        DateOnly? Date,
        PathAndFolder PathAndFolder);

}
