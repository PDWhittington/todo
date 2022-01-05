using System.Diagnostics.CodeAnalysis;
using Todo.Contracts.Data.Commands;

namespace Todo.CommandFactories;

[SuppressMessage("ReSharper", "UnusedType.Global")]
public class PushCommandFactory : CommandFactoryBase<PushCommand>
{
    private static readonly string[] Words = { "push" };

    public override bool IsDefaultCommandFactory => false;

    public override string[] HelpText => new[]
    {
        "Executes a git push.",

        "Usage: todo push"
    };


    public PushCommandFactory() : base(Words) { }

    public override PushCommand? TryGetCommand(string commandLine)

    {
        return !IsThisCommand(commandLine, out _)
            ? default : PushCommand.Singleton;
    }
}
