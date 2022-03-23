using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ListFilesCommandFactory : CommandFactoryBase<ListFilesCommand>
{
    private static readonly string[] Words = { "l", "list" };

    public override bool IsDefaultCommandFactory => false;

    public override string [] HelpText { get; } = {
        "Provides a list of all todo lists. Switches are as follows:-",
        "\tm -- main todo folder.",
        "\ta -- archive folder.",
        "\td -- lists relating to days.",
        "\tt -- lists relating to topics.",
        "",
        "Usage: todo l [m|a][d|t]"
    };

    public ListFilesCommandFactory(IOutputWriter outputWriter)
        : base(outputWriter, Words)
    { }

    public override ListFilesCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        GetListParameters(restOfCommand!, out var fileLocation, out var fileType);

        return ListFilesCommand.Of(fileLocation, fileType);
    }

    private static void GetListParameters(string restOfCommand, out OutputFolderEnum outputFolder,
        out ListFileTypeEnum listFileType)
    {
        var elements = restOfCommand
            .Split(' ', StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.CurrentCultureIgnoreCase);

        var containsM = elements.Contains("m");
        var containsA = elements.Contains("a");
        var containsD = elements.Contains("d");
        var containsT = elements.Contains("t");

        outputFolder = (containsM, containsA) switch
        {
            //Both flags or neither implies both folders should be picked up.
            (true, true) => OutputFolderEnum.MainFolder |
                            OutputFolderEnum.ArchiveFolder,
            (false, false) => OutputFolderEnum.MainFolder |
                              OutputFolderEnum.ArchiveFolder,

            //One flag but not the other implies only one folder be picked up.
            (true, false) => OutputFolderEnum.MainFolder,
            (false, true) => OutputFolderEnum.ArchiveFolder
        };

        listFileType = (containsD, containsT) switch
        {
            //Both flags or neither implies lists of both types be picked up.
            (true, true) => ListFileTypeEnum.DayList |
                            ListFileTypeEnum.TopicList,
            (false, false) => ListFileTypeEnum.DayList |
                              ListFileTypeEnum.TopicList,

            //One flag but not the other implies only one folder be picked up.
            (true, false) => ListFileTypeEnum.DayList,
            (false, true) => ListFileTypeEnum.TopicList
        };
    }
}
