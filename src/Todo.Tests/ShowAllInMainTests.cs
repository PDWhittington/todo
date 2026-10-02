using System;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Todo.CommandFactories;
using Todo.Contracts.Data.CommandLine;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.AppLaunching;
using Todo.Contracts.Services.FileSystem;
using Todo.Contracts.Services.UI;
using Todo.Execution;

namespace Todo.Tests;

[TestFixture]
public class ShowAllInMainCommandFactoryTests
{
    [TestCase("all")]
    [TestCase("showallinmain")]
    [TestCase("ALL")]
    [TestCase("ShowAllInMain")]
    public void TryGetCommand_recognises_the_command_word(string commandWord)
    {
        var factory = CreateFactory();

        var command = factory.TryGetCommand(CommandLineInfo.Of(commandWord, string.Empty));

        Assert.That(command, Is.EqualTo(new ShowAllInMainCommand()));
    }

    [Test]
    public void TryGetCommand_returns_null_for_another_command()
    {
        var factory = CreateFactory();

        var command = factory.TryGetCommand(CommandLineInfo.Of("list", string.Empty));

        Assert.That(command, Is.Null);
    }

    [Test]
    public void TryGetCommand_rejects_anything_after_the_command_word()
    {
        var factory = CreateFactory();

        var exception = Assert.Throws<ArgumentException>(() =>
            factory.TryGetCommand(CommandLineInfo.Of("all", "extra")));

        Assert.That(exception!.Message, Is.EqualTo("Command expects nothing following."));
    }

    private static ShowAllInMainCommandFactory CreateFactory()
        => new(Substitute.For<IOutputWriter>());
}

[TestFixture]
public class ShowAllInMainCommandExecutorTests
{
    [Test]
    public void Execute_launches_main_folder_day_lists_in_date_then_path_order()
    {
        var fileListCreator = Substitute.For<IFileListCreator>();
        var fileOpener = Substitute.For<ITextFileLauncher>();
        var later = DayList("/todos/2026-04-07.md", new DateOnly(2026, 4, 7));
        var sameDayLaterPath = DayList("/todos/b.md", new DateOnly(2026, 4, 1));
        var sameDayEarlierPath = DayList("/todos/a.md", new DateOnly(2026, 4, 1));

        fileListCreator
            .GetFiles<DayListFilePathInfo>(OutputFolderEnum.MainFolder, ListFileTypeEnum.DayList)
            .Returns([later, sameDayLaterPath, sameDayEarlierPath]);

        var executor = CreateExecutor(fileListCreator, fileOpener, Substitute.For<IOutputWriter>());

        executor.Execute(new ShowAllInMainCommand());

        Received.InOrder(() =>
        {
            fileOpener.LaunchFiles("/todos/a.md");
            fileOpener.LaunchFiles("/todos/b.md");
            fileOpener.LaunchFiles("/todos/2026-04-07.md");
        });
        fileListCreator.Received(1).GetFiles<DayListFilePathInfo>(
            OutputFolderEnum.MainFolder, ListFileTypeEnum.DayList);
    }

    [Test]
    public void Execute_when_there_are_no_day_lists_reports_that_and_does_not_launch()
    {
        var fileListCreator = Substitute.For<IFileListCreator>();
        var fileOpener = Substitute.For<ITextFileLauncher>();
        var outputWriter = Substitute.For<IOutputWriter>();

        fileListCreator
            .GetFiles<DayListFilePathInfo>(OutputFolderEnum.MainFolder, ListFileTypeEnum.DayList)
            .Returns([]);

        var executor = CreateExecutor(fileListCreator, fileOpener, outputWriter);

        executor.Execute(new ShowAllInMainCommand());

        outputWriter.Received(1).WriteLine("There are no day todo lists in the main folder.");
        fileOpener.DidNotReceive().LaunchFiles(Arg.Any<string[]>());
    }

    private static ShowAllInMainCommandExecutor CreateExecutor(
        IFileListCreator fileListCreator,
        ITextFileLauncher fileOpener,
        IOutputWriter outputWriter)
        => new(
            fileOpener,
            fileListCreator,
            outputWriter,
            Substitute.For<ILogger<ShowAllInMainCommandExecutor>>());

    private static DayListFilePathInfo DayList(string path, DateOnly date)
        => DayListFilePathInfo.Of(path, FileTypeEnum.MarkdownDayList, FolderEnum.TodoRoot, date);
}
