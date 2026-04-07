using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.FileSystem.Paths;

namespace Todo.FileSystem;

public class FileListCreator : IFileListCreator
{
    private readonly IDateListPathResolver _dateListPathResolver;
    private readonly IOutputFolderPathProvider _pathRootingProvider;

    public FileListCreator(IDateListPathResolver dateListPathResolver,
        IOutputFolderPathProvider pathRootingProvider)
    {
        _dateListPathResolver = dateListPathResolver;
        _pathRootingProvider = pathRootingProvider;
    }

    private struct PathAndFolder
    {
        public string Path;
        public FolderEnum Folder;
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
        .Select(pathAndFolder => FilePathInfo.Of(pathAndFolder.PathAndFolder.Path,
            MapToFileTypeEnum(pathAndFolder.FileType), pathAndFolder.PathAndFolder.Folder))
        .ToArray();

        return pathsInRelevantFolders;

        FileTypeEnum MapToFileTypeEnum(ListFileTypeEnum lfi)
            => lfi switch
            {
                ListFileTypeEnum.DayList => FileTypeEnum.MarkdownDayList,
                ListFileTypeEnum.TopicList => FileTypeEnum.MarkdownTopicList,
                _ => throw new Exception()
            };

        (bool Match, ListFileTypeEnum FileType, PathAndFolder PathAndFolder) CategoriseAndMatch(PathAndFolder pathAndFolder)
        {
            var fileName = Path.GetFileName(pathAndFolder.Path);

            var regExMatch = Regex.Match(fileName, pattern, RegexOptions.None);

            var isDayList = regExMatch.Success;

            var match = isDayList && listFileType.HasFlag(ListFileTypeEnum.DayList) ||
                        !isDayList && listFileType.HasFlag(ListFileTypeEnum.TopicList);

            var fileType = isDayList ? ListFileTypeEnum.DayList : ListFileTypeEnum.TopicList;

            return (match, fileType, pathAndFolder);
        }
    }
}
