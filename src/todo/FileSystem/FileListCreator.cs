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

    struct PathAndFolder
    {
        public string Path;
        public FolderEnum Folder;
    }

    public FilePathInfo[] GetFiles(OutputFolderEnum outputFolder,
        ListFileTypeEnum listFileType)
    {
        var pattern = _dateListPathResolver.GetRegExForThisFileType();

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

        FileTypeEnum MapToFileTypeEnum(ListFileTypeEnum listFileType)
            => listFileType switch
            {
                ListFileTypeEnum.DayList => FileTypeEnum.MarkdownDayList,
                ListFileTypeEnum.TopicList => FileTypeEnum.MarkdownTopicList,
                _ => throw new Exception()
            };

        var pathsInRelevantFolders = new []
            {
                outputFolder.HasFlag(OutputFolderEnum.MainFolder)
                    ? Directory.GetFiles(_pathRootingProvider.GetRootedOutputFolder())
                        .Select(path => new PathAndFolder { Path = path, Folder = FolderEnum.TodoRoot })
                    : Array.Empty<PathAndFolder>(),

                outputFolder.HasFlag(OutputFolderEnum.ArchiveFolder)
                    ? Directory.GetFiles(_pathRootingProvider.GetRootedArchiveFolder())
                        .Select(path => new PathAndFolder { Path = path, Folder = FolderEnum.Archive })
                    : Array.Empty<PathAndFolder>(),

            }.SelectMany(x => x)
            .Select(CategoriseAndMatch)
            .Where(filterInfo => filterInfo.Match)
            .Select(pathAndFolder => FilePathInfo.Of(pathAndFolder.PathAndFolder.Path,
                MapToFileTypeEnum(pathAndFolder.FileType), pathAndFolder.PathAndFolder.Folder))
            .ToArray();

        return pathsInRelevantFolders;
    }
}
