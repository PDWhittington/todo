namespace Todo.Contracts.Data.CommandLine;

public record CommandLineInfo
{
    public string Command { get; }

    public string Switches { get; }

    private CommandLineInfo(string command, string switches)
    {
        Command = command;
        Switches = switches;
    }

    public static CommandLineInfo Of(string command, string switches)
        => new(command, switches);

    public static CommandLineInfo FromArgs(string[] args)
    {
        // args[0] is the process/binary. User input starts at args[1].
        if (args.Length <= 1)
            return Of(string.Empty, string.Empty);

        var command = args[1];
        var switches = args.Length == 2
            ? string.Empty
            : string.Join(' ', args[2..]);

        return Of(command, switches);
    }

    public override string ToString()
        => string.IsNullOrEmpty(Switches) ? Command : $"{Command} {Switches}";
}
