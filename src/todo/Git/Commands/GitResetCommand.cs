namespace Todo.Git.Commands;

public class GitResetCommand : GitCommandBase
{
    public bool Hard { get; }

    public GitResetCommand(bool hard = false)
    {
        Hard = hard;
    }

    internal override string GetCommand() => Hard ? "reset --hard" : "reset";
}
