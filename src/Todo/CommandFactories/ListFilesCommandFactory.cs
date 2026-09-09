using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Todo.Contracts.Data.CommandLine;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Data.FileSystem;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ListFilesCommandFactory(IOutputWriter outputWriter, IBareModeProvider bareModeProvider)
    : CommandFactoryBase<ListFilesCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["l", "list"];

    public override bool IsDefaultCommandFactory => false;

    protected override string [] HelpText { get; } =
    [
        "Provides a list of all todo lists. Switches may be separated by spaces or concatenated. " +
        "Switches are as follows:-",
        "\tm -- main todo folder.",
        "\ta -- archive folder.",
        "\td -- lists relating to days.",
        "\tt -- lists relating to topics.",
        "\tb -- bare output; print only file paths, with no headings or other boilerplate."
    ];

    protected override string Usage => "l [m|a][d|t][b]";

    public override ListFilesCommand? TryGetCommand(CommandLineInfo commandLine)
    {
        if (!IsThisCommand(commandLine)) return null;

        GetListParameters(commandLine.Switches, out var fileLocation, out var fileType, out var bare);

        if (bare) bareModeProvider.IsBare = true;

        return ListFilesCommand.Of(fileLocation, fileType, bare);
    }

    private static void GetListParameters(string restOfCommand, out OutputFolderEnum outputFolder,
        out ListFileTypeEnum listFileType, out bool bare)
    {
        var flags = restOfCommand
            .Where(c => !char.IsWhiteSpace(c))
            .Select(char.ToLowerInvariant)
            .ToHashSet();

        var containsM = flags.Contains('m');
        var containsA = flags.Contains('a');
        var containsD = flags.Contains('d');
        var containsT = flags.Contains('t');
        bare = flags.Contains('b');

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

            //One flag but not the other implies only one type be picked up.
            (true, false) => ListFileTypeEnum.DayList,
            (false, true) => ListFileTypeEnum.TopicList
        };
    }
}
