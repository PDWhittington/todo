using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowWebpageCommandFactory(
    IOutputWriter outputWriter,
    IConstantsProvider constantsProvider)
    : CommandFactoryBase<ShowWebpageCommand>(outputWriter, Words)
{
    private static readonly string[] Words = ["w", "web", "www"];

    public override bool IsDefaultCommandFactory => false;

    protected override string[] HelpText { get; } =
    [
        $"Opens the project page ({constantsProvider.ProjectWebsite}) in the default browser."
    ];

    protected override string Usage => "w";

    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public override ShowWebpageCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return null;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException($"{nameof(ShowWebpageCommand)} expects nothing following.");

        return ShowWebpageCommand.Singleton;
    }
}