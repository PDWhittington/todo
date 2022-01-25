using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Todo.Contracts.Data.Commands;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ListFilesCommandFactory : CommandFactoryBase<ListFilesCommand>
{
    private static readonly string[] Words = { "l", "list" };

    public override bool IsDefaultCommandFactory => false;

    public override string [] HelpText => new[]
    {
        "Provides a list of all todo lists. Switches are as follows:-",
        "\tm -- main todo folder.",
        "\ta -- archive folder.",
        "\td -- lists relating to days.",
        "\tt -- lists relating to topics.",
        "",
        "Usage: todo l [m|a][d|t]"
    };

    public ListFilesCommandFactory()
        : base(Words)
    { }

    public override ListFilesCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        GetListParameters(restOfCommand!, out var fileLocation, out var fileType);

        return ListFilesCommand.Of(fileLocation, fileType);
    }

    private static void GetListParameters(string restOfCommand, out ListFilesCommand.FileLocationEnum fileLocation,
        out ListFilesCommand.FileTypeEnum fileType)
    {
        var elements = restOfCommand
            .Split(' ', StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.CurrentCultureIgnoreCase);

        var containsM = elements.Contains("m");
        var containsA = elements.Contains("a");
        var containsD = elements.Contains("d");
        var containsT = elements.Contains("t");

        fileLocation = (containsM, containsA) switch
        {
            //Both flags or neither implies both folders should be picked up.
            (true, true) => ListFilesCommand.FileLocationEnum.MainFolder |
                            ListFilesCommand.FileLocationEnum.ArchiveFolder,
            (false, false) => ListFilesCommand.FileLocationEnum.MainFolder |
                              ListFilesCommand.FileLocationEnum.ArchiveFolder,

            //One flag but not the other implies only one folder be picked up.
            (true, false) => ListFilesCommand.FileLocationEnum.MainFolder,
            (false, true) => ListFilesCommand.FileLocationEnum.ArchiveFolder,
        };

        fileType = (containsD, containsT) switch
        {
            //Both flags or neither implies lists of both types be picked up.
            (true, true) => ListFilesCommand.FileTypeEnum.DayList |
                            ListFilesCommand.FileTypeEnum.TopicList,
            (false, false) => ListFilesCommand.FileTypeEnum.DayList |
                              ListFilesCommand.FileTypeEnum.TopicList,

            //One flag but not the other implies only one folder be picked up.
            (true, false) => ListFilesCommand.FileTypeEnum.DayList,
            (false, true) => ListFilesCommand.FileTypeEnum.TopicList,
        };
    }
}
