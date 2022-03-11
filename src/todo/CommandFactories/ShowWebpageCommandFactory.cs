using System;
using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;
using Todo.Contracts.Services.StateAndConfig;
using Todo.Contracts.Services.UI;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class ShowWebpageCommandFactory : CommandFactoryBase<ShowWebpageCommand>
{
    private static readonly string[] Words = { "w", "web", "www" };

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText { get; } 

    public ShowWebpageCommandFactory(IOutputWriter outputWriter, 
        IConstantsProvider constantsProvider) 
        : base(outputWriter, Words) 
    {
        HelpText = new [] {
            $"Opens the project page ({constantsProvider.ProjectWebsite}) in the default browser",
            "",
            "Usage: todo w"
        };
    }

    public override ShowWebpageCommand? TryGetCommand(string commandLine)
    {
        if (!IsThisCommand(commandLine, out var restOfCommand)) return default;

        if (!string.IsNullOrWhiteSpace(restOfCommand))
            throw new ArgumentException($"{nameof(ShowWebpageCommand)} expects nothing following.");

        return ShowWebpageCommand.Singleton;
    }
}
