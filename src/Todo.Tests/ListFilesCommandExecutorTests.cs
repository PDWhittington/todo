using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.UI;
using Todo.Execution;
using Todo.FileSystem;

namespace Todo.Tests;

[TestFixture]
public class ListFilesCommandExecutorTests
{
    [Test]
    public void BareModeWritesOnlySortedPaths()
    {
        var fileListCreator = Substitute.For<IFileListCreator>();
        var outputWriter = Substitute.For<IOutputWriter>();
        var logger = Substitute.For<ILogger<ListFilesCommandExecutor>>();
        var fileSystem = new WindowsFileSystem();

        fileListCreator
            .GetFiles<FilePathInfo>(Arg.Any<OutputFolderEnum>(), Arg.Any<ListFileTypeEnum>())
            .Returns(
            [
                FilePathInfo.Of(@"C:\todos\b.md", FileTypeEnum.MarkdownDayList, FolderEnum.TodoRoot, fileSystem),
                FilePathInfo.Of(@"C:\todos\a.md", FileTypeEnum.MarkdownDayList, FolderEnum.TodoRoot, fileSystem)
            ]);

        var executor = new ListFilesCommandExecutor(fileListCreator, outputWriter, logger);
        executor.Execute(ListFilesCommand.Of(OutputFolderEnum.MainFolder, ListFileTypeEnum.DayList, true));

        outputWriter.DidNotReceive().WriteLine(Arg.Any<string>());
        Received.InOrder(() =>
        {
            outputWriter.WriteLineAlways(@"C:\todos\a.md");
            outputWriter.WriteLineAlways(@"C:\todos\b.md");
        });
    }

    [Test]
    public void NonBareModeWritesBoilerplateViaWriteLine()
    {
        var fileListCreator = Substitute.For<IFileListCreator>();
        var outputWriter = Substitute.For<IOutputWriter>();
        var logger = Substitute.For<ILogger<ListFilesCommandExecutor>>();

        var fileSystem = new WindowsFileSystem();

        fileListCreator
            .GetFiles<FilePathInfo>(Arg.Any<OutputFolderEnum>(), Arg.Any<ListFileTypeEnum>())
            .Returns(
            [
                FilePathInfo.Of(@"C:\todos\a.md", FileTypeEnum.MarkdownDayList, FolderEnum.TodoRoot, fileSystem)
            ]);

        var executor = new ListFilesCommandExecutor(fileListCreator, outputWriter, logger);
        executor.Execute(ListFilesCommand.Of(OutputFolderEnum.MainFolder, ListFileTypeEnum.DayList));

        outputWriter.DidNotReceive().WriteLineAlways(Arg.Any<string>());
        outputWriter.Received(1).WriteLine(Arg.Is<string>(s =>
            s.Contains("Listing") && s.Contains(@"C:\todos\a.md")));
    }
}
